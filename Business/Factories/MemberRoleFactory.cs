using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class MemberRoleFactory
{
    public static MemberRole? Map(MemberRoleEntity entity)
    {
        if (entity == null) return null;

        var memberRole = new MemberRole
        {
            Id = entity.Id,
            MemberRoleName = entity.MemberRoleName
        };

        return memberRole;
    }
}
