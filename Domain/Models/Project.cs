namespace Domain.Models;

public class Project
{
    public int Id { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Budget { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public Client Client { get; set; } = new();
    public AppUser ProjectOwner { get; set; } = new();
    public ProjectStatus ProjectStatus { get; set; } = new();
}
