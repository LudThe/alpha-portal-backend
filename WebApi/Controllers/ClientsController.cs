using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ClientsController(IClientService clientService) : ControllerBase
{
    private readonly IClientService _clientService = clientService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _clientService.GetAll();
        return Ok(clients);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _clientService.GetById(id);
        if (client == null) return NotFound();
        return Ok(client);
    }


    [UseAdminApiKey]
    [SwaggerRequestExample(typeof(ClientRegistrationForm), typeof(ClientRegistrationExample))]
    [HttpPost]
    public async Task<IActionResult> Create(ClientRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _clientService.CreateAsync(form);

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
    [SwaggerRequestExample(typeof(ClientRegistrationForm), typeof(ClientRegistrationExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ClientRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _clientService.UpdateAsync(id, form);

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
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _clientService.RemoveAsync(id);

        return result.StatusCode switch
        {
            200 => Ok(result),
            404 => NotFound(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }

    [UseAdminApiKey]
    [HttpDelete("bulk")]
    public async Task<IActionResult> RemoveMultiple([FromBody] List<int> ids)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var results = await _clientService.RemoveMultipleAsync(ids);

        if (results.All(r => r.StatusCode == 200))
        {
            return Ok(results);
        }

        if (results.Any(r => r.StatusCode == 404))
        {
            return NotFound();
        }

        if (results.Any(r => r.StatusCode == 409))
        {
            return Conflict(results);
        }

        return Problem();
    }
}
