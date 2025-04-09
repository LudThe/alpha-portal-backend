using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ServiceResult> CreateAsync(ClientRegistrationForm form);
        Task<IEnumerable<Client>> GetAll();
        Task<Client?> GetById(int id);
        Task<ServiceResult> RemoveAsync(int id);
        Task<List<ServiceResult>> RemoveMultipleAsync(List<int> ids);
        Task<ServiceResult> UpdateAsync(int id, ClientRegistrationForm form);
    }
}