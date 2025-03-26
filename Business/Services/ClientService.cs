using Business.Factories;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class ClientService(ClientRepository clientRepository, ClientInformationRepository clientInformationRepository, ClientAddressRepository clientAddressRepository)
{
    private readonly ClientRepository _clientRepository = clientRepository;
    private readonly ClientInformationRepository _clientInformationRepository = clientInformationRepository;
    private readonly ClientAddressRepository _clientAddressRepository = clientAddressRepository;


    public async Task<IEnumerable<Client>> GetAll()
    {
        var list = await _clientRepository.GetAllAsync(
            selector: x => ClientFactory.Map(x)!
        );

        return list.OrderBy(x => x.Id);
    }


    public async Task<Client?> GetById(int id)
    {
        var clientEntity = await _clientRepository.GetAsync(
                predicate: x => x.Id == id
            );

        if (clientEntity == null) return null;

        var client = ClientFactory.Map(clientEntity);
        return client;
    }


    public async Task<ServiceResult> CreateAsync(ClientRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();


        if (await _clientRepository.ExistsAsync(x => x.ClientName == form.ClientName))
            return ServiceResult.AlreadyExists();

        try
        {
            var clientEntity = ClientFactory.Create(form);
            var result = await _clientRepository.AddAsync(clientEntity!);
            if (!result)
                return ServiceResult.Failed();

            await _clientInformationRepository.AddAsync(clientEntity!.ContactInformation);

            await _clientAddressRepository.AddAsync(clientEntity.Address);

            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> UpdateAsync(int id, ClientRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        var clientEntity = await _clientRepository.GetAsync(x => x.Id == id);

        if (clientEntity == null) return ServiceResult.NotFound();

        try
        {
            var updatedClientEntity = ClientFactory.Update(clientEntity, form);
            var result = await _clientRepository.UpdateAsync(clientEntity);
            if (!result)
                return ServiceResult.Failed();

            await _clientInformationRepository.UpdateAsync(updatedClientEntity!.ContactInformation);

            await _clientAddressRepository.UpdateAsync(updatedClientEntity.Address);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }


    public async Task<ServiceResult> RemoveAsync(int id)
    {
        var clientEntity = await _clientRepository.GetAsync(x => x.Id == id);

        if (clientEntity == null) return ServiceResult.NotFound();

        try
        {
            var result = await _clientRepository.RemoveAsync(clientEntity);
            if (!result)
                return ServiceResult.Failed();

            await _clientInformationRepository.RemoveAsync(clientEntity.ContactInformation);

            await _clientAddressRepository.RemoveAsync(clientEntity.Address);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }
}