using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class ProjectStatusService(IProjectStatusRepository projectStatusRepository) : IProjectStatusService
{
    private readonly IProjectStatusRepository _projectStatusRepository = projectStatusRepository;

    public async Task<IEnumerable<ProjectStatus>> GetAll()
    {
        var entities = await _projectStatusRepository.GetAllAsync(sortBy: x => x.Id);
        var projectStatuses = entities.Select(ProjectStatusFactory.Map);

        return projectStatuses!;
    }

    public async Task<ProjectStatus?> GetByStatusNameAsync(string statusName)
    {
        var entity = await _projectStatusRepository.GetAsync(x => x.StatusName == statusName);
        if (entity == null) return null;

        var projectStatus = ProjectStatusFactory.Map(entity);
        return projectStatus;
    }
}
