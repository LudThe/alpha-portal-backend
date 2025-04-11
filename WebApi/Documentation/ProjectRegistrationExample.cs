using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public class ProjectRegistrationExample : IExamplesProvider<ProjectRegistrationForm>
{
    public ProjectRegistrationForm GetExamples()
    {
        return new ProjectRegistrationForm
        {
            ProjectName = "Example name",
            Description = "Example description",
            Budget = 1200,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            ClientId = 1,
            AppUserId = "d946efc3-c80f-4e64-9877-25527be09f66",
            ProjectStatusId = 1
        };
    }
}
