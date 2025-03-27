using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController(ProjectService projectService) : ControllerBase
{
    private readonly ProjectService _projectService = projectService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAll();
        return Ok(projects);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetById(id);
        if (project == null) return NotFound();
        return Ok(project);
    }


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
            _ => Problem(),
        };
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _projectService.RemoveAsync(id);

        return result.StatusCode switch
        {
            200 => Ok(result),
            _ => Problem(),
        };
    }
}
