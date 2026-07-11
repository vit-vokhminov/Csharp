namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Тип реализации репозитория локаций.
/// </summary>
public enum LocationRepositoryType
{
    /// <summary>
    /// Реализация через EF Core.
    /// </summary>
    EfCore,

    /// <summary>
    /// Реализация через Dapper + Npgsql.
    /// </summary>
    Dapper
}