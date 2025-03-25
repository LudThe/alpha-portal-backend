using Data.Entities;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class ClientService(ClientRepository clientRepository, ClientInformationRepository clientInformationRepository, ClientAddressRepository clientAddressRepository)
{
    private readonly ClientRepository _clientRepository = clientRepository;
    private readonly ClientInformationRepository _clientInformationRepository = clientInformationRepository;
    private readonly ClientAddressRepository _clientAddressRepository = clientAddressRepository;

    public async Task<ServiceResult> CreateAsync(ClientRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _clientRepository.ExistsAsync(x => x.ClientName == form.ClientName))
            return ServiceResult.AlreadyExists();

        try
        {
            var clientEntity = new ClientEntity(); // replace with factory ClientFactory.Create(form)
            var result = await _clientRepository.AddAsync(clientEntity);
            if (!result)
                return ServiceResult.Failed();

            await _clientInformationRepository.AddAsync(clientEntity.ContactInformation);

            await _clientAddressRepository.AddAsync(clientEntity.Address);

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}