using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProjectStatusesController(IProjectStatusService projectStatusService) : ControllerBase
{
    private readonly IProjectStatusService _projectStatusService = projectStatusService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projectStatuses = await _projectStatusService.GetAll();
        return Ok(projectStatuses);
    }
}
