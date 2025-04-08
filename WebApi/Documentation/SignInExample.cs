using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public class SignInExample : IExamplesProvider<SignInForm>
{
    public SignInForm GetExamples()
    {
        return new SignInForm
        {
            Email = "john.smith@mail.com",
            Password = "Password@123",
        };
    }
}
