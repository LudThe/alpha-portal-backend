using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation;

namespace WebApi.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("api/[controller]")]
[ApiController]
public class SignInController(IAppUserService appUserService) : ControllerBase
{
    private readonly IAppUserService _appUserService = appUserService;


    [SwaggerRequestExample(typeof(SignInForm), typeof(SignInExample))]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appUserService.SignIn(form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            401 => Unauthorized(result),
            _ => Problem(),
        };
    }
}
