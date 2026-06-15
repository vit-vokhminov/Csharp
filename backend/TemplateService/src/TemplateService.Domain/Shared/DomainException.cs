namespace TemplateService.Domain.Primitives;

/// <summary>
/// Базовое исключение домена. Выбрасывается при нарушении бизнес‑правил и инвариантов.
/// </summary>
public class DomainException : Exception
{
    private const string DefaultErrorCode = "domain.error";

    /// <summary>
    /// Код ошибки для программной обработки.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DomainException"/> с кодом ошибки по умолчанию.
    /// </summary>
    public DomainException() : this(DefaultErrorCode)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DomainException"/> с указанным сообщением.
    /// Использует код ошибки по умолчанию.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public DomainException(string message) : this(DefaultErrorCode, message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DomainException"/> с указанным кодом ошибки и сообщением.
    /// </summary>
    /// <param name="errorCode">Код ошибки для программной обработки.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    public DomainException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DomainException"/> с сообщением и внутренним исключением.
    /// Использует код ошибки по умолчанию.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public DomainException(string message, Exception innerException)
        : this(DefaultErrorCode, message, innerException)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DomainException"/> с кодом ошибки, сообщением и внутренним исключением.
    /// </summary>
    /// <param name="errorCode">Код ошибки для программной обработки.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public DomainException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
    }
}
