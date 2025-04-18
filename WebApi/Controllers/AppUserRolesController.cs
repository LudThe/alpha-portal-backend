﻿using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
[UseAdminApiKey]
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
