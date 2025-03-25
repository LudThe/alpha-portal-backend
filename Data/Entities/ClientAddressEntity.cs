using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ClientAddressEntity
{
    [Key]
    public int ClientId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}