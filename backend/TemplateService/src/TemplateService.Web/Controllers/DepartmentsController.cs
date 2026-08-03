using Microsoft.AspNetCore.Mvc;
using TemplateService.Contracts.Departments;
using TemplateService.Core.DepartmentLocations;
using TemplateService.Core.Departments;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly CreateDepartmentUseCase _createDepartmentUseCase;
    private readonly UpdateDepartmentUseCase _updateDepartmentUseCase;
    private readonly LinkLocationToDepartmentUseCase _linkLocationToDepartmentUseCase;
    private readonly UnlinkLocationFromDepartmentUseCase _unlinkLocationFromDepartmentUseCase;

    public DepartmentsController(
        CreateDepartmentUseCase createDepartmentUseCase,
        UpdateDepartmentUseCase updateDepartmentUseCase,
        LinkLocationToDepartmentUseCase linkLocationToDepartmentUseCase,
        UnlinkLocationFromDepartmentUseCase unlinkLocationFromDepartmentUseCase)
    {
        _createDepartmentUseCase = createDepartmentUseCase;
        _updateDepartmentUseCase = updateDepartmentUseCase;
        _linkLocationToDepartmentUseCase = linkLocationToDepartmentUseCase;
        _unlinkLocationFromDepartmentUseCase = unlinkLocationFromDepartmentUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var createRequest = new CreateDepartmentRequest
            {
                Name = request.Name,
                Slug = request.Slug,
                ParentId = request.ParentId,
                LocationIds = request.LocationIds ?? Array.Empty<Guid>(),
            };

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
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(DepartmentResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken = default)
    {
        return Ok(Array.Empty<DepartmentResponse>());
    }

    /// <summary>
    /// Обновляет название подразделения (PATCH).
    /// Не меняет parentId и path — только name.
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var department = await _updateDepartmentUseCase.ExecuteAsync(id, request.Name, cancellationToken);

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

            return Ok(response);
        }
        catch (DepartmentNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Привязывает локацию к подразделению.
    /// </summary>
    [HttpPost("{id:guid}/locations/{locationId:guid}")]
    [ProducesResponseType(typeof(DepartmentLocationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> LinkLocation(
        Guid id,
        Guid locationId,
        [FromBody] LinkLocationRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var isPrimary = request?.IsPrimary ?? false;
            var departmentLocation = await _linkLocationToDepartmentUseCase.ExecuteAsync(
                id, locationId, isPrimary, cancellationToken);

            var response = new DepartmentLocationResponse
            {
                Id = departmentLocation.Id,
                DepartmentId = departmentLocation.DepartmentId,
                LocationId = departmentLocation.LocationId,
                IsPrimary = departmentLocation.IsPrimary,
                CreatedAt = departmentLocation.CreatedAt,
                UpdatedAt = departmentLocation.UpdatedAt,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = departmentLocation.DepartmentId },
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
        catch (DepartmentLocationAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Отвязывает локацию от подразделения.
    /// </summary>
    [HttpDelete("{id:guid}/locations/{locationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnlinkLocation(
        Guid id,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _unlinkLocationFromDepartmentUseCase.ExecuteAsync(id, locationId, cancellationToken);
            return NoContent();
        }
        catch (DepartmentLocationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

/// <summary>
/// Запрос на привязку локации к подразделению. 
/// </summary>
public sealed class LinkLocationRequest
{
    /// <summary>
    /// Признак основной локации.
    /// </summary>
    public bool IsPrimary { get; init; }
}

/// <summary>
/// Ответ с данными связи подразделения с локацией. 
/// </summary>
public sealed class DepartmentLocationResponse
{
    /// <summary>
    /// Идентификатор связи.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Идентификатор подразделения.
    /// </summary>
    public required Guid DepartmentId { get; init; }

    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    public required Guid LocationId { get; init; }

    /// <summary>
    /// Признак основной локации подразделения.
    /// </summary>
    public required bool IsPrimary { get; init; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public required DateTime UpdatedAt { get; init; }
}
