using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
