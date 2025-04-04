using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AppUserAddressEntity
{
    [PersonalData]
    [Key, ForeignKey(nameof(AppUser))]
    public string AppUserId { get; set; } = null!;
    public virtual AppUserEntity AppUser { get; set; } = null!;

    [PersonalData]
    public string? StreetAddress { get; set; }
    [PersonalData]
    public string? PostalCode { get; set; }
    [PersonalData]
    public string? City { get; set; }
}
