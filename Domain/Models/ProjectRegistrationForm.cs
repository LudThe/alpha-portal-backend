namespace Domain.Models;

public class ProjectRegistrationForm
{
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public double? Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ClientId { get; set; }
    public int MemberId { get; set; }
    public int ProjectStatusId { get; set; }
}
