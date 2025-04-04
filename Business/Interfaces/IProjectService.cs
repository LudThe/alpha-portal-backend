using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> CreateAsync(ProjectRegistrationForm form);
        Task<IEnumerable<Project>> GetAll();
        Task<Project?> GetById(int id);
        Task<ServiceResult> RemoveAsync(int id);
        Task<ServiceResult> UpdateAsync(int id, ProjectRegistrationForm form);
    }
}