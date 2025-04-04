using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class AppUserProfileRepository(DataContext context) : BaseRepository<AppUserProfileEntity>(context), IAppUserProfileRepository
{
}
