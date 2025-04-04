using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public class ProjectFactory
{
    public static Project? Map(ProjectEntity entity)
    {
        if (entity == null) return null;


        var client = ClientFactory.Map(entity.Client);

        var appUser = AppUserFactory.Map(entity.AppUser);

        var status = ProjectStatusFactory.Map(entity.ProjectStatus);

        var project = new Project
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            Budget = entity.Budget,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Created = entity.Created,
            Modified = entity.Modified,
            Client = client!,
            ProjectOwner = appUser!,
            ProjectStatus = status!,
        };

        return project;
    }


    public static ProjectEntity? Create(ProjectRegistrationForm form)
    {
        if (form == null) return null;

        DateTime dateTime = DateTime.Now;

        var project = new ProjectEntity
        {
            ProjectName = form.ProjectName,
            Description = form.Description,
            ImageUrl = form.ImageUrl,
            Budget = form.Budget,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            Created = dateTime,
            Modified = dateTime,
            ClientId = form.ClientId!,
            AppUserId = form.AppUserId!,
            ProjectStatusId = form.ProjectStatusId
        };

        return project;
    }


    public static ProjectEntity? Update(ProjectEntity projectEntity, ProjectRegistrationForm form)
    {
        if (form == null) return null;

        projectEntity.ProjectName = form.ProjectName;
        projectEntity.Description = form.Description;
        projectEntity.ImageUrl = form.ImageUrl;
        projectEntity.Budget = form.Budget;

        projectEntity.StartDate = form.StartDate;
        projectEntity.EndDate = form.EndDate;

        projectEntity.Modified = DateTime.UtcNow;

        projectEntity.ClientId = form.ClientId!;
        projectEntity.AppUserId = form.AppUserId!;
        projectEntity.ProjectStatusId = form.ProjectStatusId;

        return projectEntity;
    }
}
