using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(MemberRoleName), IsUnique = true)]
public class MemberRoleEntity
{
    [Key]
    public int Id { get; set; }
    public string MemberRoleName { get; set; } = null!;
}