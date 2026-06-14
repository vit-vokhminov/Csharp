namespace SeatReservation.Domain.Events;

/// <summary>
/// Класс события (мероприятие)
/// </summary>
public class Event
{
    public Event(Guid id, Guid venueId, EventDetails eventDetails, string name, DateTime eventDate)
    {
        Id = id;
        VenueId = venueId;
        EventDetails = eventDetails;
        Name = name;
        EventDate = eventDate;
    }

    /// <summary>
    /// id события
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Навигационное свойство на таблицу EventDetails
    /// </summary>
    public EventDetails EventDetails { get; private set; }

    /// <summary>
    /// id площадки
    /// </summary>
    public Guid VenueId { get; private set; }

    /// <summary>
    /// Название события
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Время события
    /// </summary>
    public DateTime EventDate { get; private set; }
}
