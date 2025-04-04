using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ClientAddressEntity
{
    [Key, ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}