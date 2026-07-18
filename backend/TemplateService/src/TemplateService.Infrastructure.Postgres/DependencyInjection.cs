using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using TemplateService.Core.DepartmentLocations;
using TemplateService.Core.Departments;
using TemplateService.Core.Locations;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Методы расширения для регистрации инфраструктурных сервисов.
/// </summary>
public static class InfrastructureServices
{
    /// <summary>
    /// Регистрирует инфраструктурные сервисы.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="repositoryType">Тип реализации репозитория локаций.</param>
    /// <returns>Коллекция сервисов для продолжения настройки.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        LocationRepositoryType repositoryType = LocationRepositoryType.EfCore)
    {
        // Регистрация DbContext для EF Core
        services.AddDbContext<TemplateServiceDbContext>((serviceProvider, options) =>
        {
            string? connectionString = configuration.GetConnectionString("postgres");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'postgres' is not configured.");
            }

            options.UseNpgsql(connectionString);
        });

        // Регистрация NpgsqlDataSource для Dapper
        services.AddSingleton<NpgsqlDataSource>(serviceProvider =>
        {
            string? connectionString = configuration.GetConnectionString("postgres");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'postgres' is not configured.");
            }

            return NpgsqlDataSource.Create(connectionString);
        });

        // Регистрация репозитория в зависимости от выбранного типа 
        switch (repositoryType)
        {
            case LocationRepositoryType.EfCore:
                services.AddScoped<ILocationRepository, LocationRepository>();
                break;

            case LocationRepositoryType.Dapper:
                services.AddScoped<ILocationRepository, DapperLocationRepository>();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(repositoryType), repositoryType, null);
        }

        // Ркгистрация репозиториев подразделений и связей
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentLocationRepository, DepartmentLocationRepository>();

        return services;
    }
}
