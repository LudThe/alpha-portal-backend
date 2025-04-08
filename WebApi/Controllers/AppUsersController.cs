using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using WebApi.Documentation;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AppUsersController(IAppUserService appUserService) : ControllerBase
{
    private readonly IAppUserService _appUserService = appUserService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appUsers = await _appUserService.GetAll();
        return Ok(appUsers);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var appUser = await _appUserService.GetById(id);
        if (appUser == null) return NotFound();
        return Ok(appUser);
    }


    [HttpGet("signedInInfo"), Authorize]
    public async Task<IActionResult> GetByJwtToken()
    {
        var appUserId = User?.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
        var appUser = await _appUserService.GetById(appUserId!);
        if (appUser == null) return NotFound();
        return Ok(appUser);
    }

    [UseAdminApiKey]
    [SwaggerRequestExample(typeof(AppUserRegistrationForm), typeof(AppUserRegistrationExample))]
    [HttpPost]
    public async Task<IActionResult> CreateNoPassword(AppUserRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appUserService.CreateWithoutPasswordAsync(form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            201 => Created(),
            400 => BadRequest(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }


    [UseAdminApiKey]
    [SwaggerRequestExample(typeof(AppUserRegistrationForm), typeof(AppUserRegistrationExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, AppUserRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appUserService.UpdateAsync(id, form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => Problem(),
        };
    }

    [UseAdminApiKey]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appUserService.RemoveAsync(id);

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }
}
