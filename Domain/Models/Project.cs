namespace Domain.Models;

public class Project
{
    public int Id { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public double? Budget { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public Client Client { get; set; } = new();
    public Member ProjectOwner { get; set; } = new();
    public ProjectStatus ProjectStatus { get; set; } = new();
}
