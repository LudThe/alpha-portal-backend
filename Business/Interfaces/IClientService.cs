using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ServiceResult> CreateAsync(ClientRegistrationForm form);
        Task<IEnumerable<Client>> GetAll();
        Task<Client?> GetById(string id);
        Task<ServiceResult> RemoveAsync(string id);
        Task<ServiceResult> UpdateAsync(string id, ClientRegistrationForm form);
    }
}