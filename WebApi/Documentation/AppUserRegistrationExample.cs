using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public class AppUserRegistrationExample : IExamplesProvider<AppUserRegistrationForm>
{
    public AppUserRegistrationForm GetExamples()
    {
        return new AppUserRegistrationForm
        {
            FirstName = "John",
            LastName = "Smith",
            JobTitle = "Developer",
            Email = "john.smith@mail.com",
            Phone = "0701234567",
            StreetAddress = "Example street 3",
            PostalCode = "15320",
            City = "Stockholm",
            AppUserRole = "User"
        };
    }
}
