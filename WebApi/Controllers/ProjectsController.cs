using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation;

namespace WebApi.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;


    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAll();
        return Ok(projects);
    }


    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetById(id);
        if (project == null) return NotFound();
        return Ok(project);
    }


    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    [SwaggerRequestExample(typeof(ProjectRegistrationForm), typeof(ProjectRegistrationExample))]
    [HttpPost]
    public async Task<IActionResult> Create(ProjectRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _projectService.CreateAsync(form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            201 => Created(),
            400 => BadRequest(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }


    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    [SwaggerRequestExample(typeof(ProjectRegistrationForm), typeof(ProjectRegistrationExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProjectRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _projectService.UpdateAsync(id, form);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => Problem(),
        };
    }


    [Produces("application/json")]
    [Consumes("application/json")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _projectService.RemoveAsync(id);

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(result),
            _ => Problem(),
        };
    }
}
