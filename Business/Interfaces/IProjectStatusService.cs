using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectStatusService
    {
        Task<IEnumerable<ProjectStatus>> GetAll();
        Task<ProjectStatus?> GetByStatusNameAsync(string statusName);
    }
}