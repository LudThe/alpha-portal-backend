using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class AppUserFactory
{
    public static AppUser? Map(AppUserEntity entity)
    {
        if (entity == null) return null;

        var profile = new AppUserProfile
        {
            FirstName = entity?.AppUserProfile?.FirstName,
            LastName = entity?.AppUserProfile?.LastName,
            JobTitle = entity?.AppUserProfile?.JobTitle,
            Phone = entity?.AppUserProfile?.Phone,
            ImageUrl = entity?.AppUserProfile?.ImageUrl,
            Created = entity?.AppUserProfile?.Created,
            Modified = entity?.AppUserProfile?.Modified
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


    public static MemberEntity? Create(MemberRegistrationForm form)
    {
        if (form == null) return null;

        DateTime dateTime = DateTime.Now;

        var contact = new MemberInformationEntity
        {
            Email = form.Email,
            Phone = form.Phone
        };

        var address = new MemberAddressEntity
        {
            StreetAddress = form.StreetAddress,
            PostalCode = form.PostalCode,
            City = form.City,
        };

        var member = new MemberEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            JobTitle = form.JobTitle,
            Created = dateTime,
            Modified = dateTime,
            ContactInformation = contact,
            Address = address,
            MemberRoleId = form.MemberRoleId,
        };

        return member;
    }


    public static MemberEntity? Update(MemberEntity memberEntity, MemberRegistrationForm form)
    {
        if (form == null) return null;

        memberEntity.FirstName = form.FirstName;
        memberEntity.LastName = form.LastName;
        memberEntity.JobTitle = form.JobTitle;
        memberEntity.Modified = DateTime.UtcNow;

        // memberEntity.ContactInformation.Email = form.Email;
        memberEntity.ContactInformation.Phone = form.Phone;

        memberEntity.Address.StreetAddress = form.StreetAddress;
        memberEntity.Address.PostalCode = form.PostalCode;
        memberEntity.Address.City = form.City;

        memberEntity.MemberRoleId = form.MemberRoleId;

        return memberEntity;
    }
}
