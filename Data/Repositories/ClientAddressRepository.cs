using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ClientAddressRepository(DataContext context) : BaseRepository<ClientAddressEntity>(context), IClientAddressRepository
{
}
