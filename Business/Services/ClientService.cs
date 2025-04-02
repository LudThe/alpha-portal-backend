using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IClientInformationRepository clientInformationRepository, IClientAddressRepository clientAddressRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IClientInformationRepository _clientInformationRepository = clientInformationRepository;
    private readonly IClientAddressRepository _clientAddressRepository = clientAddressRepository;

    public async Task<IEnumerable<Client>> GetAll()
    {
        var entities = await _clientRepository.GetAllAsync(
                orderByDescending: true,
                sortBy: x => x.Created,
                filterBy: null,
                i => i.ContactInformation,
                i => i.Address
        );

        var clients = entities.Select(ClientFactory.Map);

        return clients!;
    }


    public async Task<Client?> GetById(int id)
    {
        var clientEntity = await _clientRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.ContactInformation,
                i => i.Address
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
            return ServiceResult.Conflict();

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

        var clientEntity = await _clientRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.ContactInformation,
                i => i.Address
         );

        if (clientEntity == null) return ServiceResult.NotFound();

        try
        {
            var updatedClientEntity = ClientFactory.Update(clientEntity, form);
            var result = await _clientRepository.UpdateAsync(updatedClientEntity!);
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
        var clientEntity = await _clientRepository.GetAsync(
                findBy: x => x.Id == id,
                i => i.Projects
            );

        if (clientEntity == null) return ServiceResult.NotFound();

        try
        {
            // can't remove if connected to project
            var hasProjects = clientEntity.Projects.Count != 0;
            if (hasProjects)
                return ServiceResult.Conflict();

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