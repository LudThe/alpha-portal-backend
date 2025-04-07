using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ClientRegistrationForm
{
    [Required]
    public string ClientName { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$")]
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? ImageUrl { get; set; }
    public string? Reference { get; set; }

    [Required]
    public string StreetAddress { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;
}
