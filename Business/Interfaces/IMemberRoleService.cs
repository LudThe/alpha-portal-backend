using Domain.Models;

namespace Business.Interfaces
{
    public interface IMemberRoleService
    {
        Task<IEnumerable<MemberRole>> GetAll();
    }
}