using Business.Factories;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class ProjectService(ProjectRepository projectRepository)
{
    private readonly ProjectRepository _projectRepository = projectRepository;

    public async Task<IEnumerable<Project>> GetAll()
    {
        var list = await _projectRepository.GetAllAsync(
            selector: x => ProjectFactory.Map(x)!
        );

        return list.OrderBy(x => x.Id);
    }


    public async Task<Project?> GetById(int id)
    {
        var projectEntity = await _projectRepository.GetAsync(
                predicate: x => x.Id == id
            );

        if (projectEntity == null) return null;

        var project = ProjectFactory.Map(projectEntity);
        return project;
    }


    public async Task<ServiceResult> CreateAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _projectRepository.ExistsAsync(x => x.ProjectName == form.ProjectName))
            return ServiceResult.AlreadyExists();

        try
        {
            var projectEntity = ProjectFactory.Create(form);
            var result = await _projectRepository.AddAsync(projectEntity!);
            if (!result)
                return ServiceResult.Failed();

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

        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

        if (projectEntity == null) return ServiceResult.NotFound();

        try
        {
            var updatedProjectEntity = ProjectFactory.Update(projectEntity, form);
            var result = await _projectRepository.UpdateAsync(updatedProjectEntity!);
            if (!result)
                return ServiceResult.Failed();

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

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}
