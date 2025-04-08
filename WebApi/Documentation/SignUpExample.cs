using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public class SignUpExample : IExamplesProvider<SignUpForm>
{
    public SignUpForm GetExamples()
    {
        return new SignUpForm
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@mail.com",
            Password = "Password@123",
        };
    }
}
