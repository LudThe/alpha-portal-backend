using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class MemberFactory
{
    public static Member? Map(MemberEntity entity)
    {
        if (entity == null) return null;

        var contact = new MemberInformation
        {
            Email = entity.ContactInformation.Email,
            Phone = entity.ContactInformation?.Phone,
        };

        var address = new MemberAddress
        {
            StreetAddress = entity.Address.StreetAddress,
            PostalCode = entity.Address.PostalCode,
            City = entity.Address.City
        };

        var role = MemberRoleFactory.Map(entity.MemberRole);

        var member = new Member
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Created = entity.Created,
            JobTitle = entity.JobTitle,
            Modified = entity.Modified,
            MemberInformation = contact,
            MemberAddress = address,
            MemberRole = role!
        };

        return member;
    }


    public static MemberEntity? Create(AppUserRegistrationForm form)
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
        };

        return member;
    }


    public static MemberEntity? Update(MemberEntity memberEntity, AppUserRegistrationForm form)
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


        return memberEntity;
    }
}
