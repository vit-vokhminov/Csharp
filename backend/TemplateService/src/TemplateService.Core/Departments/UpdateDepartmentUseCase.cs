using TemplateService.Domain.Departments;
namespace TemplateService.Core.Departments;

/// <summary>
/// Use case для обновления существующего подразделения.
/// </summary>
public sealed class UpdateDepartmentUseCase
{
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateDepartmentUseCase(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    /// <summary>
    /// Обновляет название подразделения.
    /// Не меняет parentId и path — только name.
    /// </summary>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <param name="newName">Новое название подразделения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновлённое подразделение.</returns>
    /// <exception cref="DepartmentNotFoundException">Выбрасывается, если подразделение не найдено.</exception>
    public async Task<Department> ExecuteAsync(
        Guid departmentId,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            throw new DepartmentNotFoundException(departmentId);
        }

        // Вызываем доменный метод Rename — он меняет только Name и UpdatedAt 
        department.Rename(newName, DateTime.UtcNow);

        await _departmentRepository.UpdateAsync(department, cancellationToken);

        return department;
    }
}
