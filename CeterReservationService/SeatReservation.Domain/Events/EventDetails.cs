namespace SeatReservation.Domain.Events;

/// <summary>
/// Класс детали события (мероприятие)
/// </summary>
public class EventDetails
{
    public EventDetails(int capacity, int description)
    {
        Capacity = capacity;
        Description = description;
    }

    /// <summary>
    /// id значения в таблице детали события
    /// </summary>
    public Guid EventId { get; } = Guid.Empty;

    /// <summary>
    /// Максимальная вместимость на площадке
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Описание мероприятия
    /// </summary>
    public int Description { get; private set; }
}