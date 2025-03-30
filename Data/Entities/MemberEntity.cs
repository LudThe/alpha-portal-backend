using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual MemberInformationEntity ContactInformation { get; set; } = null!;
    public virtual MemberAddressEntity Address { get; set; } = null!;

    public int MemberRoleId { get; set; }
    public virtual MemberRoleEntity MemberRole { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
