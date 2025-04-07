using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SignInController(IAppUserService appUserService) : ControllerBase
{
    private readonly IAppUserService _appUserService = appUserService;

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
