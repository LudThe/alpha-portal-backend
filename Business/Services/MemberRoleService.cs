using Data.Repositories;

namespace Business.Services;

public class MemberRoleService(MemberRoleRepository memberRoleRepository)
{
    private readonly MemberRoleRepository _memberRoleRepository = memberRoleRepository;


}
