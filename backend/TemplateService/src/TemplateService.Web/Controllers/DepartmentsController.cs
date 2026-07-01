using Microsoft.AspNetCore.Mvc;
using TemplateService.Contracts.Departments;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] CreateDepartmentRequest request)
    {
        var response = new DepartmentResponse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Path = request.ParentId.HasValue ? $"parent/{request.Slug}" : request.Slug,
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(DepartmentResponse[]), StatusCodes.Status200OK)]
    public IActionResult GetList()
    {
        return Ok(Array.Empty<DepartmentResponse>());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        var response = new DepartmentResponse
        {
            Id = id,
            Name = request.Name,
            Slug = "slug",
            Path = request.ParentId.HasValue ? $"parent/slug" : "slug",
            ParentId = request.ParentId,
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