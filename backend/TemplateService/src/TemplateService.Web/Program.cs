using Scalar.AspNetCore;
using TemplateService.Core;
using TemplateService.Infrastructure.Postgres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Нативный OpenAPI .NET 9/10 
builder.Services.AddOpenApi();

// Для тестового контроллера 
builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCoreServices();

// Чтение типа репозитория из конфигурации (по умолчанию EfCore)

var repositoryTypeString = builder.Configuration["LocationRepositoryType"] ?? "EfCore";
if (!Enum.TryParse<LocationRepositoryType>(repositoryTypeString, ignoreCase: true, out var repositoryType))
{
    repositoryType = LocationRepositoryType.EfCore;
}

// Регистрация инфраструктурных сервисов с выбранным типом репозитория
builder.Services.AddInfrastructureServices(builder.Configuration, repositoryType);

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

// MVC контроллеры 
app.MapControllers();

app.MapHealthChecks("/health");

// Если среда не prod

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();   //	генерируем спецификацию
    app.MapScalarApiReference();	// маппим Scalar эндпоинты

}

await app.RunAsync();
