namespace Domain.Models;

public class ProjectRegistrationForm
{
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ClientId { get; set; }
    public string? AppUserId { get; set; }
    public int ProjectStatusId { get; set; }
}
