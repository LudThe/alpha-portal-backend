using Business.Factories;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class MemberRoleService(MemberRoleRepository memberRoleRepository)
{
    private readonly MemberRoleRepository _memberRoleRepository = memberRoleRepository;


    public async Task<IEnumerable<MemberRole>> GetAll()
    {
        var list = await _memberRoleRepository.GetAllAsync(
            selector: x => MemberRoleFactory.Map(x)!
        );

        return list.OrderBy(x => x.Id);
    }
}
