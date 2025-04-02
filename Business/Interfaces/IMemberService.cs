using Domain.Models;

namespace Business.Interfaces
{
    public interface IMemberService
    {
        Task<ServiceResult> CreateAsync(MemberRegistrationForm form);
        Task<IEnumerable<Member>> GetAll();
        Task<Member?> GetById(int id);
        Task<ServiceResult> RemoveAsync(int id);
        Task<ServiceResult> UpdateAsync(int id, MemberRegistrationForm form);
    }
}