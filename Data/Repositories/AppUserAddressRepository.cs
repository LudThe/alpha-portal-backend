using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class AppUserAddressRepository(DataContext context) : BaseRepository<AppUserAddressEntity>(context), IAppUserAddressRepository
{
}
