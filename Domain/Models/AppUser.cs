namespace Domain.Models;

public class AppUser
{
    public string Id { get; set; } = null!;
    public AppUserAddress AppUserAddress { get; set; } = new();
    public AppUserProfile AppUserProfile { get; set; } = new();
}

public class AppUserProfile
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
}

public class AppUserAddress
{
    public string? StreetAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}

