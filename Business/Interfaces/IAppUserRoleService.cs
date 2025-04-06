using Domain.Models;

namespace Business.Interfaces
{
    public interface IAppUserRoleService
    {
        Task<IEnumerable<AppUserRole>> GetAll();
    }
}