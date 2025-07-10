using Microsoft.AspNetCore.Mvc;
using Route.Domain.Contracts.Services;
using Route.Domain.Entities;

namespace Route.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController(IRouteService service) : ControllerBase
{
    private readonly IRouteService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Routes>>> GetAll()
        => Ok(await _service.GetAllRoutesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Routes>> GetById(int id)
    {
        var route = await _service.GetRouteByIdAsync(id);
        if (route == null) return NotFound();
        return Ok(route);
    }
    [HttpGet("search")]
    public async Task<IActionResult> GetBestRoute([FromQuery] string origin, [FromQuery] string destination)
    {
        var result = await _service.CalculateBestRoute(origin, destination);
        if (result.Contains("not found"))
            return NotFound(result);

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Routes route)
    {
        var created = await _service.CreateRouteAsync(route);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Routes route)
    {
        if (id != route.Id) return BadRequest();

        var success = await _service.UpdateRouteAsync(route);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteRouteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
