using Data.Repositories;

namespace Business.Services;

public class ProjectStatusService(ProjectStatusRepository projectStatusRepository)
{
    private readonly ProjectStatusRepository _projectStatusRepository = projectStatusRepository;
}
