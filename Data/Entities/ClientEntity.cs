using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int Id { get; set; }
    public string ClientName { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsActive { get; set; }

    public virtual ClientInformationEntity ContactInformation { get; set; } = null!;
    public virtual ClientAddressEntity Address { get; set; } = null!;

}