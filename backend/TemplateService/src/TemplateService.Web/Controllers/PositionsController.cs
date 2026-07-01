using Microsoft.AspNetCore.Mvc;
using TemplateService.Contracts.Positions;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(PositionResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] CreatePositionRequest request)
    {
        var response = new PositionResponse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PositionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(PositionResponse[]), StatusCodes.Status200OK)]
    public IActionResult GetList()
    {
        return Ok(Array.Empty<PositionResponse>());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PositionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdatePositionRequest request)
    {
        var response = new PositionResponse
        {
            Id = id,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return NoContent();
    }
}
