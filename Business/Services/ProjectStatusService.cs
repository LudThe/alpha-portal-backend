using Business.Factories;
using Business.Interfaces;
using Business.Managers;
using Data.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class ProjectStatusService(IProjectStatusRepository projectStatusRepository, IMemoryCache cache) : IProjectStatusService
{
    private readonly IProjectStatusRepository _projectStatusRepository = projectStatusRepository;
    private readonly IMemoryCache _cache = cache;

    public async Task<IEnumerable<ProjectStatus>> GetAll()
    {
        var cacheKey = "project_statuses_all";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<ProjectStatus>? cachedProjectStatuses))
            return cachedProjectStatuses!;


        var entities = await _projectStatusRepository.GetAllAsync(sortBy: x => x.Id);
        var projectStatuses = entities.Select(ProjectStatusFactory.Map);

        CacheManager.ProjectStatusKeys.Add(cacheKey);
        _cache.Set(cacheKey, projectStatuses, TimeSpan.FromMinutes(5));

        return projectStatuses!;
    }

    public async Task<ProjectStatus?> GetByStatusNameAsync(string statusName)
    {
        var cacheKey = $"project_status_{statusName}";
        if (_cache.TryGetValue(cacheKey, out ProjectStatus? cachedProjectStatus))
            return cachedProjectStatus!;


        var entity = await _projectStatusRepository.GetAsync(x => x.StatusName == statusName);
        if (entity == null) return null;

        var projectStatus = ProjectStatusFactory.Map(entity);

        CacheManager.ProjectStatusKeys.Add(cacheKey);
        _cache.Set(cacheKey, projectStatus, TimeSpan.FromMinutes(5));

        return projectStatus;
    }
}
