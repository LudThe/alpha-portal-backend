using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ProjectRegistrationForm
{
    [Required]
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Budget { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int ClientId { get; set; }
    public string? AppUserId { get; set; }
    [Required]
    public int ProjectStatusId { get; set; }
}
