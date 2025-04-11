using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class AppUserFactory
{
    public static AppUser? Map(AppUserEntity entity, string role = "")
    {
        if (entity == null) return null;

        var profile = new AppUserProfile
        {
            FirstName = entity?.AppUserProfile?.FirstName,
            LastName = entity?.AppUserProfile?.LastName,
            JobTitle = entity?.AppUserProfile?.JobTitle,
            Phone = entity?.AppUserProfile?.Phone,
            Email = entity?.Email!,
            UserRole = role,
            ImageUrl = entity?.AppUserProfile?.ImageUrl,
            Created = entity!.AppUserProfile!.Created,
            Modified = entity.AppUserProfile.Modified
        };

        var address = new AppUserAddress
        {
            StreetAddress = entity?.AppUserAddress?.StreetAddress,
            PostalCode = entity?.AppUserAddress?.PostalCode,
            City = entity?.AppUserAddress?.City
        };

        var appUser = new AppUser
        {
            Id = entity?.Id!,
            AppUserProfile = profile,
            AppUserAddress = address
        };

        return appUser;
    }


    public static AppUserEntity? SignUp(SignUpForm form)
    {
        if (form == null) return null;

        DateTime dateTime = DateTime.Now;

        var profile = new AppUserProfileEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Created = dateTime,
            Modified = dateTime
        };

        var address = new AppUserAddressEntity
        {
        };

        var appUser = new AppUserEntity
        {
            UserName = form.Email,
            Email = form.Email,
            AppUserProfile = profile,
            AppUserAddress = address
        };

        return appUser;
    }


    public static AppUserEntity? Create(AppUserRegistrationForm form)
    {
        if (form == null) return null;

        DateTime dateTime = DateTime.Now;

        var profile = new AppUserProfileEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            JobTitle = form.JobTitle,
            Phone = form.Phone,
            Created = dateTime,
            Modified = dateTime
        };

        var address = new AppUserAddressEntity
        {
            StreetAddress = form.StreetAddress,
            PostalCode = form.PostalCode,
            City = form.City
        };

        var appUser = new AppUserEntity
        {
            UserName = form.Email,
            Email = form.Email,
            AppUserProfile = profile,
            AppUserAddress = address
        };

        return appUser;
    }


    public static AppUserEntity? Update(AppUserEntity appUserEntity, AppUserRegistrationForm form)
    {
        if (form == null) return null;
        if (appUserEntity == null) return null;

        appUserEntity!.AppUserProfile!.FirstName = form.FirstName;
        appUserEntity.AppUserProfile.LastName = form.LastName;
        appUserEntity.AppUserProfile.JobTitle = form.JobTitle;
        appUserEntity.AppUserProfile.Phone = form.Phone;
        appUserEntity.AppUserProfile.Modified = DateTime.UtcNow;

        appUserEntity!.AppUserAddress!.StreetAddress = form.StreetAddress;
        appUserEntity.AppUserAddress.PostalCode = form.PostalCode;
        appUserEntity.AppUserAddress.City = form.City;

        return appUserEntity;
    }
}
