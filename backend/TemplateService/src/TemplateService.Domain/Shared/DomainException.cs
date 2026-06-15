namespace TemplateService.Domain.Primitives;

/// <summary>
/// Базовое исключение домена. Выбрасывается при нарушении бизнес-правил и инвариантов. 
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Код ошибки для программной обработки.
    /// </summary>
    public string ErrorCode { get; }

    public DomainException()
    {
        ErrorCode = "domain.error";
    }

    public DomainException(string message)
        : base(message)
    {
        ErrorCode = "domain.error";
    }

    public DomainException(string errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = "domain.error";
    }
}
