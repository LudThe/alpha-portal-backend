namespace Domain.Models;

public class Member
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    public MemberInformation MemberInformation { get; set; } = new();
    public MemberAddress MemberAddress { get; set; } = new();
    public MemberRole MemberRole { get; set; } = new();
}

public class MemberInformation
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class MemberAddress
{
    public string? StreetAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}
