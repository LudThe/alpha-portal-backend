using Business.Factories;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class ProjectStatusService(ProjectStatusRepository projectStatusRepository)
{
    private readonly ProjectStatusRepository _projectStatusRepository = projectStatusRepository;

    public async Task<IEnumerable<ProjectStatus>> GetAll()
    {
        var list = await _projectStatusRepository.GetAllAsync(
            selector: x => ProjectStatusFactory.Map(x)!
        );

        return list.OrderBy(x => x.Id);
    }
}
