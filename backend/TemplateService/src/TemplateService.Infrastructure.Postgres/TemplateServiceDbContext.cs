using Microsoft.EntityFrameworkCore;

namespace TemplateService.Infrastructure.Postgres;

public class TemplateServiceDbContext : DbContext
{
    public TemplateServiceDbContext(DbContextOptions<TemplateServiceDbContext> options)
        : base(options)
    {
    }
}