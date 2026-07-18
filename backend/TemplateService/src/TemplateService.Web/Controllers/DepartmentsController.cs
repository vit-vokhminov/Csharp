using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateService.Contracts.Departments;
using TemplateService.Core.Departments;
using TemplateService.Domain.Departments;
using ContractsCreateRequest = TemplateService.Contracts.Departments.CreateDepartmentRequest;
using CoreCreateRequest = TemplateService.Core.Departments.CreateDepartmentRequest;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly CreateDepartmentUseCase _createDepartmentUseCase;

    public DepartmentsController(CreateDepartmentUseCase createDepartmentUseCase)
    {
        _createDepartmentUseCase = createDepartmentUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] ContractsCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var createRequest = new CoreCreateRequest(
                request.Name,
                request.Slug,
                request.ParentId,
                request.LocationIds ?? Array.Empty<Guid>());

            var department = await _createDepartmentUseCase.ExecuteAsync(createRequest, cancellationToken);

            var response = new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name.Value,
                Slug = department.Slug.Value,
                Path = department.Path.Value,
                ParentId = department.ParentId,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (DepartmentNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (LocationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (SlugAlreadyTakenException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(DepartmentResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken = default)
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