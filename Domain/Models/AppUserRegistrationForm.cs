using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class AppUserRegistrationForm
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public IFormFile? ImageFile { get; set; }

    [Required]
    [RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$")]
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    [Required]
    public string StreetAddress { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string AppUserRole { get; set; } = null!;
}
