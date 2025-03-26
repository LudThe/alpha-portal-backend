using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class MemberInformationRepository(DataContext context) : BaseRepository<MemberInformationEntity>(context), IMemberInformationRepository
{
}
