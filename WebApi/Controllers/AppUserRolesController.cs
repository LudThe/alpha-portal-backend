using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppUserRolesController(IAppUserRoleService appUserRoleService) : ControllerBase
{
    private readonly IAppUserRoleService _appUserRoleService = appUserRoleService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _appUserRoleService.GetAll();
        return Ok(roles);
    }
}
