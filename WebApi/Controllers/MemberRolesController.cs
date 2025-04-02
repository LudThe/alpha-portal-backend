using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberRolesController(IMemberRoleService memberRoleService) : ControllerBase
{
    private readonly IMemberRoleService _memberRoleService = memberRoleService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var memberRoles = await _memberRoleService.GetAll();
        return Ok(memberRoles);
    }
}
