using Business.Factories;
using Business.Managers;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public interface IAppUserService
{
    Task<ServiceResult> CreateWithoutPasswordAsync(AppUserRegistrationForm form);
    Task<IEnumerable<AppUser>> GetAll();
    Task<AppUser?> GetById(string id);
    Task<ServiceResult> RemoveAsync(string id);
    Task<ServiceResult> UpdateAsync(string id, AppUserRegistrationForm form);
}

public class AppUserService(IAppUserRepository appUserRepository, IAppUserProfileRepository appUserProfileRepository, IAppUserAddressRepository appUserAddressRepository, UserManager<AppUserEntity> appUserManager, IMemoryCache cache) : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository = appUserRepository;
    private readonly IAppUserProfileRepository _appUserProfileRepository = appUserProfileRepository;
    private readonly IAppUserAddressRepository _appUserAddressRepository = appUserAddressRepository;
    private readonly UserManager<AppUserEntity> _appUserManager = appUserManager;
    private readonly IMemoryCache _cache = cache;


    private void ClearCache()
    {
        foreach (var key in CacheManager.AppUserKeys)
        {
            _cache.Remove(key);
        }
        CacheManager.AppUserKeys.Clear();
    }


    public async Task<IEnumerable<AppUser>> GetAll()
    {
        var cacheKey = "app_users_all";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<AppUser>? cachedAppUsers))
            return cachedAppUsers!;


        List<AppUser> appUsers = [];
        var entities = await _appUserRepository.GetAllAsync(
             orderByDescending: true,
                sortBy: x => x.AppUserProfile!.FirstName,
                filterBy: null,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );

        foreach (var entity in entities)
        {
            var role = "";
            var roles = await _appUserManager.GetRolesAsync(entity);

            if (roles.Count > 0) role = roles[0];

            var appUser = AppUserFactory.Map(entity, role);

            appUsers.Add(appUser!);
        }

        CacheManager.AppUserKeys.Add(cacheKey);
        _cache.Set(cacheKey, appUsers, TimeSpan.FromMinutes(5));

        return appUsers!;
    }


    public async Task<AppUser?> GetById(string id)
    {
        var cacheKey = $"client_{id}";
        if (_cache.TryGetValue(cacheKey, out AppUser? cachedAppUser))
            return cachedAppUser!;


        var appUserEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );

        if (appUserEntity == null) return null;

        var role = "";
        var roles = await _appUserManager.GetRolesAsync(appUserEntity);

        if (roles.Count > 0) role = roles[0];

        var appUser = AppUserFactory.Map(appUserEntity, role);

        CacheManager.AppUserKeys.Add(cacheKey);
        _cache.Set(cacheKey, appUser, TimeSpan.FromMinutes(5));

        return appUser;
    }


    //public async Task<ServiceResult> CreateWithPasswordAsync(AppUserRegistrationForm form)
    //{

    //}


    public async Task<ServiceResult> CreateWithoutPasswordAsync(AppUserRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _appUserRepository.ExistsAsync(x => x.Email == form.Email))
            return ServiceResult.Conflict();

        try
        {
            var appUserEntity = AppUserFactory.Create(form);
            var result = await _appUserManager.CreateAsync(appUserEntity!);
            if (!result.Succeeded)
                return ServiceResult.Failed();

            var role = form.AppUserRole.IsNullOrEmpty() ? "User" : form.AppUserRole;

            var roleResult = await _appUserManager.AddToRoleAsync(appUserEntity!, role);
            if (!roleResult.Succeeded)
                return ServiceResult.Created(message: "User was added but not assigned a role");

            ClearCache();

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> UpdateAsync(string id, AppUserRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        var appUserEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.AppUserProfile!,
                i => i.AppUserAddress!
            );

        if (appUserEntity == null) return ServiceResult.NotFound();

        try
        {
            var roles = await _appUserManager.GetRolesAsync(appUserEntity);
            var oldRole = roles[0] ?? "";
            var newRole = form.AppUserRole;

            var updatedAppUserEntity = AppUserFactory.Update(appUserEntity, form);
            var result = await _appUserManager.UpdateAsync(updatedAppUserEntity!);
            if (!result.Succeeded)
                return ServiceResult.Failed();

            if (oldRole != newRole)
            {
                // remove old role
                var removeRoleResult = await _appUserManager.RemoveFromRoleAsync(appUserEntity!, oldRole);
                if (!removeRoleResult.Succeeded)
                    return ServiceResult.Created(message: "Remove old role failed");

                // set new role
                var addRoleResult = await _appUserManager.AddToRoleAsync(appUserEntity!, newRole);
                if (!addRoleResult.Succeeded)
                    return ServiceResult.Created(message: "Set new role failed");
            }

            ClearCache();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> RemoveAsync(string id)
    {
        var appUserEntity = await _appUserRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Projects
            );

        if (appUserEntity == null) return ServiceResult.NotFound();

        try
        {
            // can't remove if connected to project
            var hasProjects = appUserEntity.Projects.Count != 0;
            if (hasProjects)
                return ServiceResult.Conflict(message: "Can't remove because the user is connected to a project");

            var result = await _appUserRepository.RemoveAsync(appUserEntity);
            if (!result)
                return ServiceResult.Failed();

            await _appUserProfileRepository.RemoveAsync(appUserEntity.AppUserProfile!);

            await _appUserAddressRepository.RemoveAsync(appUserEntity.AppUserAddress!);

            ClearCache();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}
