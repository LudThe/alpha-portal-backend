using Business.Factories;
using Business.Handlers;
using Business.Interfaces;
using Business.Managers;
using Data.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IProjectStatusService projectStatusService, IMemoryCache cache, IFileHandler fileHandler) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectStatusService _projectStatusService = projectStatusService;
    private readonly IMemoryCache _cache = cache;
    private readonly IFileHandler _fileHandler = fileHandler;

    private void ClearCache()
    {
        foreach (var key in CacheManager.ProjectKeys)
        {
            _cache.Remove(key);
        }
        CacheManager.ProjectKeys.Clear();
    }


    public async Task<IEnumerable<Project>> GetAll()
    {
        var cacheKey = "projects_all";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Project>? cachedProjects))
            return cachedProjects!;



        var entities = await _projectRepository.GetAllAsync(
                orderByDescending: true,
                sortBy: x => x.Created,
                filterBy: null,
                i => i.Client,
                i => i.Client.ContactInformation,
                i => i.Client.Address,
                i => i.AppUser,
                i => i.AppUser.AppUserProfile,
                i => i.AppUser.AppUserAddress,
                i => i.ProjectStatus
            );
        var projects = entities.Select(ProjectFactory.Map);

        CacheManager.ProjectKeys.Add(cacheKey);
        _cache.Set(cacheKey, projects, TimeSpan.FromMinutes(5));

        return projects!;
    }


    public async Task<Project?> GetById(int id)
    {
        var cacheKey = $"project_{id}";
        if (_cache.TryGetValue(cacheKey, out Project? cachedProject))
            return cachedProject!;


        var projectEntity = await _projectRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Client,
                i => i.Client.ContactInformation,
                i => i.Client.Address,
                i => i.AppUser.AppUserProfile,
                i => i.AppUser.AppUserAddress,
                i => i.ProjectStatus
            );

        if (projectEntity == null) return null;

        var project = ProjectFactory.Map(projectEntity);

        CacheManager.ProjectKeys.Add(cacheKey);
        _cache.Set(cacheKey, project, TimeSpan.FromMinutes(5));

        return project;
    }


    public async Task<ServiceResult> CreateAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _projectRepository.ExistsAsync(x => x.ProjectName == form.ProjectName))
            return ServiceResult.Conflict();

        try
        {
            var projectEntity = ProjectFactory.Create(form);

            if (form.ImageFile != null)
            {
                var imageFileUri = await _fileHandler.UploadFileAsync(form.ImageFile);
                projectEntity!.ImageUrl = imageFileUri;
            }

            var projectStatus = _projectStatusService.GetByStatusNameAsync("Active");
            if (projectStatus.Result != null && projectStatus.Result.Id != 0)
                projectEntity!.ProjectStatusId = projectStatus.Result.Id;
            else
                return ServiceResult.Failed();

            var result = await _projectRepository.AddAsync(projectEntity!);
            if (!result)
                return ServiceResult.Failed();

            ClearCache();

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }



    public async Task<ServiceResult> UpdateAsync(int id, ProjectRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        var projectEntity = await _projectRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Client,
                i => i.AppUser,
                i => i.ProjectStatus
            );

        if (projectEntity == null) return ServiceResult.NotFound();

        try
        {
            var updatedProjectEntity = ProjectFactory.Update(projectEntity, form);

            if (form.ImageFile != null)
            {
                if (updatedProjectEntity?.ImageUrl != null)
                    await _fileHandler.RemoveFileAsync(updatedProjectEntity.ImageUrl);

                var imageFileUri = await _fileHandler.UploadFileAsync(form.ImageFile);
                updatedProjectEntity!.ImageUrl = imageFileUri;
            }

            var result = await _projectRepository.UpdateAsync(updatedProjectEntity!);
            if (!result)
                return ServiceResult.Failed();

            ClearCache();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> RemoveAsync(int id)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

        if (projectEntity == null) return ServiceResult.NotFound();

        try
        {
            var result = await _projectRepository.RemoveAsync(projectEntity);
            if (!result)
                return ServiceResult.Failed();

            if (projectEntity.ImageUrl != null)
                await _fileHandler.RemoveFileAsync(projectEntity.ImageUrl);

            ClearCache();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}
