using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TemplateService.Contracts.Departments;
using TemplateService.Core.Departments;
using TemplateService.Core.Locations;

namespace TemplateService.Core;

/// <summary>
/// Методы расширения для регистрации сервисов в DI.
/// </summary>
public static class CoreServicesExtension
{
    /// <summary>
    /// Регистрирует сервисы слоя Application/Core.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов для продолжения настройки.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Регистрация FluentValidation валидаторов
        services.AddValidatorsFromAssemblyContaining<CreateLocationDtoValidator>();

        // Регистрация Use Cases
        services.AddScoped<CreateLocationUseCase>();
        services.AddScoped<CreateDepartmentUseCase>();

        // Репозиторий регистрируется в инфраструктурном слое
        // ILocationRepository, IDepartmentRepository, IDepartmentLocationRepository регистрируется в AddInfrastructureServices

        return services;
    }
}
