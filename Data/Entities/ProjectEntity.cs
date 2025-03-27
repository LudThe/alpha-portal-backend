using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public double? Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }


    public int ClientId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;


    public int MemberId { get; set; }
    public virtual MemberEntity Member { get; set; } = null!;


    public int ProjectStatusId { get; set; }
    public virtual ProjectStatusEntity ProjectStatus { get; set; } = null!;
}
