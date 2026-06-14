using CSharpFunctionalExtensions;

namespace SeatReservation.Domain.Reservations;

/// <summary>
/// Класс бронирования
/// </summary>
public class Reservation
{
    /// <summary>
    /// Забронированные места
    /// </summary>
    private List<ReservationSeat> _reservedSeats;

    public Reservation(Guid id, Guid eventId, Guid userId, IEnumerable<Guid> seatIds)
    {
        Id = id;
        EventId = eventId;
        UserId = userId;
        Status = ReservationStatus.Pending;
        CreateAt = DateTime.UtcNow;

        var reservedSeats = seatIds.Select(seatId => new ReservationSeat(Guid.NewGuid(), this, seatId)).ToList();
        _reservedSeats = reservedSeats;
    }

    /// <summary>
    /// id бронирования
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// id значения в таблице Event (на конкретное мероприятие)
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// id пользователя
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Статус бронирования
    /// </summary>
    public ReservationStatus Status { get; private set; }

    /// <summary>
    /// Время бронирования
    /// </summary>
    public DateTime CreateAt { get; private set; }

    /// <summary>
    /// Навигационное свойство на таблицу ReservationSeat
    /// </summary>
    public IReadOnlyList<ReservationSeat> Seats => _reservedSeats;
}