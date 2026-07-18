using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TemplateService.Core.Locations;
using TemplateService.Domain.Locations;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Реализация репозитория локаций через Dapper + Npgsql.
/// </summary>
public sealed partial class DapperLocationRepository : ILocationRepository
{

    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger<DapperLocationRepository> _logger;

    public DapperLocationRepository(
        NpgsqlDataSource dataSource,
        ILogger<DapperLocationRepository> logger)
    {
        _dataSource = dataSource;
        _logger = logger;
    }

    public async Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        const string sql = """
INSERT INTO directory.locations (id, name, address, createdat, updatedat)
VALUES (@Id, @Name, @Address, @CreatedAt, @UpdatedAt)
RETURNING id;
""";

        try
        {
            using var connection = _dataSource.CreateConnection();
            await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    location.Id,
                    Name = location.Name.Value,
                    Address = location.Address.Value,
                    CreatedAt = location.CreatedAt,
                    UpdatedAt = location.UpdatedAt
                },
                cancellationToken: cancellationToken));

            return location;

        }
        catch (Exception ex)
        {
            LogSaveError(ex, location.Name.Value);
            throw new InfrastructureException($"Не удалось сохранить локацию '{location.Name.Value}'", ex);
        }
    }

    public async Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken = default)
    {
        const string sql = """
SELECT EXISTS (
SELECT 1
FROM directory.locations WHERE name = @Name
);
""";

        using var connection = _dataSource.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            sql,
        new { Name = name },
        cancellationToken: cancellationToken));
    }

    public async Task<Location?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT id, name, address, createdat, updatedat
            FROM directory.locations
            WHERE id = @Id;
        """;

        using var connection = _dataSource.CreateConnection();
        var row = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Address, DateTime CreatedAt, DateTime UpdatedAt)>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        if (row == default)
        {
            return null;
        }

        var location = Location.Create(row.Id, row.Name, row.Address, row.CreatedAt);
        return location;
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения локации с именем {Name}")]

    private partial void LogSaveError(Exception ex, string name);

}
