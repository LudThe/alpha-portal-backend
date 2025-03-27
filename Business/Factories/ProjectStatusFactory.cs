using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class ProjectStatusFactory
{
    public static ProjectStatus? Map(ProjectStatusEntity entity)
    {
        if (entity == null) return null;

        var status = new ProjectStatus
        {
            Id = entity.Id,
            Status = entity.StatusName
        };

        return status;
    }
}
