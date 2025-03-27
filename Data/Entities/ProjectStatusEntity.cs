using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(StatusName), IsUnique = true)]
public class ProjectStatusEntity
{
    [Key]
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
}