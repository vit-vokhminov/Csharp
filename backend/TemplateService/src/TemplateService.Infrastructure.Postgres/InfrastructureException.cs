namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Исключение инфраструктуры для обработки ошибок доступа к данным.
/// </summary>
public sealed class InfrastructureException : Exception
{
    public InfrastructureException()
    : base("Ошибка инфраструктуры.")
    {
    }
    public InfrastructureException(string message)
    : base(message)
    {
    }

    public InfrastructureException(string message, Exception innerException)
    : base(message, innerException)
    {
    }
}
