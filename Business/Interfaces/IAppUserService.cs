using Domain.Models;

namespace Business.Interfaces;

public interface IAppUserService
{
    Task<ServiceResult> CreateWithoutPasswordAsync(AppUserRegistrationForm form);
    Task<ServiceResult> CreateWithPasswordAsync(SignUpForm form);
    Task<IEnumerable<AppUser>> GetAll();
    Task<AppUser?> GetById(string id);
    Task<ServiceResult> RemoveAsync(string id);
    Task<ServiceResult> SignIn(SignInForm form);
    Task<ServiceResult> UpdateAsync(string id, AppUserRegistrationForm form);
}