namespace TemplateService.Core.Locations;

/// <summary>
/// Исключение, выбрасываемое при попытке создать локацию с уже занятым именем.
/// </summary>
public sealed class LocationNameTakenException : Exception
{
    /// <summary>
    /// Имя локации, которое уже занято.
    /// </summary>
    public string? Name { get; }

    public LocationNameTakenException()
        : base("Локация с таким именем уже существует.")
    {
    }

    public LocationNameTakenException(string name)
        : base($"Локация с именем '{name}' уже существует.")
    {
        Name = name;
    }

    public LocationNameTakenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
