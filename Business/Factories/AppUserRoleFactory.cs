using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Factories;

public class AppUserRoleFactory
{
    public static AppUserRole? Map(IdentityRole entity)
    {
        if (entity == null) return null;

        var appUserRole = new AppUserRole
        {
            Id = entity.Id,
            RoleName = entity.Name
        };

        return appUserRole;
    }
}
