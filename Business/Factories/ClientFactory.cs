using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class ClientFactory
{
    public static Client? Map(ClientEntity entity)
    {
        if (entity == null) return null;

        var contact = new ClientInformation
        {
            Email = entity.ContactInformation.Email,
            Phone = entity.ContactInformation?.Phone,
            Reference = entity.ContactInformation?.Reference
        };

        var address = new ClientAddress
        {
            StreetAddress = entity.Address.StreetAddress,
            PostalCode = entity.Address.PostalCode,
            City = entity.Address.City
        };

        var client = new Client
        {
            Id = entity.Id,
            ClientName = entity.ClientName,
            Created = entity.Created,
            Modified = entity.Modified,
            IsActive = entity.IsActive,
            ClientInformation = contact,
            ClientAddress = address
        };

        return client;
    }


    public static ClientEntity? Create(ClientRegistrationForm form)
    {
        if (form == null) return null;

        DateTime dateTime = DateTime.Now;

        var contact = new ClientInformationEntity
        {
            Email = form.Email,
            Phone = form.Phone,
            Reference = form.Reference,
        };

        var address = new ClientAddressEntity
        {
            StreetAddress = form.StreetAddress,
            PostalCode = form.PostalCode,
            City = form.City,
        };

        var client = new ClientEntity
        {
            ClientName = form.ClientName,
            Created = dateTime,
            Modified = dateTime,
            IsActive = true,
            ContactInformation = contact,
            Address = address,
        };

        return client;
    }


    public static ClientEntity? Update(ClientEntity clientEntity, ClientRegistrationForm form)
    {
        if (form == null) return null;

        clientEntity.ClientName = form.ClientName;
        clientEntity.Modified = DateTime.UtcNow;

        clientEntity.ContactInformation.Email = form.Email;
        clientEntity.ContactInformation.Phone = form.Phone;
        clientEntity.ContactInformation.Reference = form.Reference;

        clientEntity.Address.StreetAddress = form.StreetAddress;
        clientEntity.Address.PostalCode = form.PostalCode;
        clientEntity.Address.City = form.City;

        return clientEntity;
    }
}
