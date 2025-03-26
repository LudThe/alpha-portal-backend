using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class MemberInformationEntity
{
    [Key]
    public int MemberId { get; set; }
    public virtual MemberEntity Member { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
}
