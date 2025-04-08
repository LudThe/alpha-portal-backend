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
public class SignUpController(IAppUserService appUserService) : ControllerBase
{
    private readonly IAppUserService _appUserService = appUserService;


    [SwaggerRequestExample(typeof(SignUpForm), typeof(SignUpExample))]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appUserService.CreateWithPasswordAsync(form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            201 => Created(),
            400 => BadRequest(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }
}
