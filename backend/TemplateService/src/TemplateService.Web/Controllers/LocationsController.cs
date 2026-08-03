using Microsoft.AspNetCore.Mvc;
using TemplateService.Contracts.Locations;
using TemplateService.Core.Departments;
using TemplateService.Core.Locations;
using FluentValidation;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly CreateLocationUseCase _createLocationUseCase;
    private readonly UpdateLocationUseCase _updateLocationUseCase;

    public LocationsController(
        CreateLocationUseCase createLocationUseCase,
        UpdateLocationUseCase updateLocationUseCase
        )
    {
        _createLocationUseCase = createLocationUseCase;
        _updateLocationUseCase = updateLocationUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var dto = new CreateLocationDto(
            request.Name,
            request.Address.Street,
            request.Address.City,
            request.Address.ZipCode,
            request.Address.Country);

        try
        {
            var id = await _createLocationUseCase.ExecuteAsync(dto, cancellationToken);

            var response = new LocationResponse
            {
                Id = id,
                Name = request.Name,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка валидации",
                Detail = ex.Message,
            });
        }
        catch (LocationNameTakenException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Имя локации уже занято",
                Detail = ex.Message,
            });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(LocationResponse[]), StatusCodes.Status200OK)]
    public IActionResult GetList()
    {
        return Ok(Array.Empty<LocationResponse>());
    }

    /// <summary>
    /// Обновляет название и адрес локации (PATCH).
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(LocationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateLocationRequest request,
    CancellationToken cancellationToken)
    {
        try
        {
            var location = await _updateLocationUseCase.ExecuteAsync(
                id,
            request.Name,
            $"{request.Address.Street}, {request.Address.City}, {request.Address.ZipCode}, {request.Address.Country}", cancellationToken);

            var response = new LocationResponse
            {
                Id = location.Id,
                Name = location.Name.Value,
                Address = new AddressDto
                {
                    Street = request.Address.Street,
                    City = request.Address.City,
                    ZipCode = request.Address.ZipCode,
                    Country = request.Address.Country,
                },
                CreatedAt = location.CreatedAt,
                UpdatedAt = location.UpdatedAt,
            };

            return Ok(response);
        }
        catch (LocationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return NoContent();
    }
}
