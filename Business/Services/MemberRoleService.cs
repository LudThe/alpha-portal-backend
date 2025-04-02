using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class MemberRoleService(IMemberRoleRepository memberRoleRepository) : IMemberRoleService
{
    private readonly IMemberRoleRepository _memberRoleRepository = memberRoleRepository;


    public async Task<IEnumerable<MemberRole>> GetAll()
    {
        var entities = await _memberRoleRepository.GetAllAsync(sortBy: x => x.Id);
        var memberRoles = entities.Select(MemberRoleFactory.Map);

        return memberRoles!;
    }
}
