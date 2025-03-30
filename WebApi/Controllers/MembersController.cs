using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembersController(MemberService memberService) : ControllerBase
{
    private readonly MemberService _memberService = memberService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _memberService.GetAll();
        return Ok(members);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var member = await _memberService.GetById(id);
        if (member == null) return NotFound();
        return Ok(member);
    }


    [HttpPost]
    public async Task<IActionResult> Create(MemberRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.CreateAsync(form);

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
    public async Task<IActionResult> Update(int id, MemberRegistrationForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.UpdateAsync(id, form);

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

        var result = await _memberService.RemoveAsync(id);

        return result.StatusCode switch
        {
            200 => Ok(result),
            409 => Conflict(result),
            _ => Problem(),
        };
    }
}
