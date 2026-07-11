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
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<DapperLocationRepository> _logger;
// В конструкторе создаётся одно соединение _dbConnection на весь срок жизни репозитория, но оно не освобождается (нет Dispose). Лучше создавать и освобождать соединение внутри каждого метода через using var connection = dataSource.CreateConnection(), чтобы избежать потенциальной утечки.
    public DapperLocationRepository(
        NpgsqlDataSource dataSource,
        ILogger<DapperLocationRepository> logger)
    {
        _dbConnection = dataSource.CreateConnection();
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
            await _dbConnection.ExecuteAsync(
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
    FROM directory.locations
    WHERE name = @Name
);
""";

        return await _dbConnection.ExecuteScalarAsync<bool>(
            sql,
            new { Name = name },
            cancellationToken: cancellationToken));
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения локации с именем {Name}")]
    private partial void LogSaveError(Exception ex, string name);
}
