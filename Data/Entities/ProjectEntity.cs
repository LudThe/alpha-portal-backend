using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    [Column(TypeName = "money")]
    public decimal? Budget { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;


    [ForeignKey(nameof(AppUser))]
    public string AppUserId { get; set; } = null!;
    public virtual AppUserEntity AppUser { get; set; } = null!;


    [ForeignKey(nameof(ProjectStatus))]
    public int ProjectStatusId { get; set; }
    public virtual ProjectStatusEntity ProjectStatus { get; set; } = null!;
}
