using Business.Factories;
using Business.Interfaces;
using Business.Managers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class AppUserRoleService(RoleManager<IdentityRole> roleManager, IMemoryCache cache) : IAppUserRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMemoryCache _cache = cache;

    public async Task<IEnumerable<AppUserRole>> GetAll()
    {
        var cacheKey = "app_user_roles_all";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<AppUserRole>? cachedRoles))
            return cachedRoles!;

        var entities = await _roleManager.Roles.ToListAsync();
        var roles = entities.Select(AppUserRoleFactory.Map);

        CacheManager.AppUserRoleKeys.Add(cacheKey);
        _cache.Set(cacheKey, roles, TimeSpan.FromMinutes(5));

        return roles!;
    }
}
