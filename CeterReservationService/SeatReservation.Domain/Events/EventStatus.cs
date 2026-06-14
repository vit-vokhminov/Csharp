namespace SeatReservation.Domain.Events;

/// <summary>
/// Статусы бронирования
/// </summary>
public enum EventStatus
{
    Planned,
    InProgress,
    Finished,
    Cancelled
}