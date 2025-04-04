using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AppUserProfileEntity
{
    [PersonalData]
    [Key, ForeignKey(nameof(AppUser))]
    public string AppUserId { get; set; } = null!;
    public virtual AppUserEntity AppUser { get; set; } = null!;


    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }

    [ProtectedPersonalData]
    public string? Phone { get; set; }

    [ProtectedPersonalData]
    public string? ImageUrl { get; set; }

    [PersonalData]
    public DateTime Created { get; set; }

    [PersonalData]
    public DateTime Modified { get; set; }
}
