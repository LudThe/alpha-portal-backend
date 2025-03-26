using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberRolesController(MemberRoleService memberRoleService) : ControllerBase
{
    private readonly MemberRoleService _memberRoleService = memberRoleService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var memberRoles = await _memberRoleService.GetAll();
        return Ok(memberRoles);
    }
}
