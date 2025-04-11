using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation;

public class ClientRegistrationExample : IExamplesProvider<ClientRegistrationForm>
{
    public ClientRegistrationForm GetExamples()
    {
        return new ClientRegistrationForm
        {
            ClientName = "John Smith",
            Email = "john.smith@mail.com",
            Phone = "0701234567",
            Reference = "Reference name",
            StreetAddress = "Example street 3",
            PostalCode = "15320",
            City = "Stockholm",
        };
    }
}
