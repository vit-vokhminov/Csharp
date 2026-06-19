using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TemplateService.Infrastructure.Postgres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Нативный OpenAPI .NET 9/10
builder.Services.AddOpenApi();

// Для тестового контроллера
builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<TemplateServiceDbContext>((serviceProvider, options) =>
{
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string configurationString = configuration.GetConnectionString("Postgress")!;
    options.UseNpgsql(configurationString);
});

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

// MVC контроллеры
app.MapControllers();

app.MapHealthChecks("/health");

// Если среда не prod
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();              // генерируем спецификацию
    app.MapScalarApiReference();   // маппим Scalar эндпоинты
}

await app.RunAsync();
