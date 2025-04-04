using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> CreateAsync(ProjectRegistrationForm form);
        Task<IEnumerable<Project>> GetAll();
        Task<Project?> GetById(string id);
        Task<ServiceResult> RemoveAsync(string id);
        Task<ServiceResult> UpdateAsync(string id, ProjectRegistrationForm form);
    }
}