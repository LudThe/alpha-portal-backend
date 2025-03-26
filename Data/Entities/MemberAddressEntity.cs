using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberAddressEntity
{
    [Key]
    public int MemberId { get; set; }
    public virtual MemberEntity Member { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
