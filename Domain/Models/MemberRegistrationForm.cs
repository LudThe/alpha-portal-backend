namespace Domain.Models;

public class MemberRegistrationForm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string? ImageUrl { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string StreetAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public int MemberRoleId { get; set; }
}
