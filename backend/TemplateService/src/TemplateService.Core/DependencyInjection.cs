using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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

        // Регистрация заглушки репозитория (реализация будет добавлена в инфраструктурном слое)
        services.AddScoped<ILocationRepository, InMemoryLocationRepository>();

        return services;
    }
}
