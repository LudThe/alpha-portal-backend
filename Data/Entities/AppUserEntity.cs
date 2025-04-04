using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class AppUserEntity : IdentityUser
{
    public AppUserProfileEntity? AppUserProfile { get; set; }
    public AppUserAddressEntity? AppUserAddress { get; set; }
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
