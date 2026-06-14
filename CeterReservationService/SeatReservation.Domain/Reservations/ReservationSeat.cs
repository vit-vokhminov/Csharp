namespace SeatReservation.Domain.Reservations;

/// <summary>
/// Класс связывающий таблицы Reservation и Seat
/// </summary>
public class ReservationSeat
{
    public ReservationSeat(Guid id, Reservation reservation, Guid seatId)
    {
        Id = id;
        Reservation = reservation;
        SeatId = seatId;
        ReservedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// id связывающий таблицы Reservation и Seat
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Прямая ссылка на Reservation
    /// </summary>
    public Reservation Reservation { get; private set; }

    /// <summary>
    /// id значения в таблице Seat
    /// </summary>
    public Guid SeatId { get; private set; }

    /// <summary>
    /// Время бронирования
    /// </summary>
    public DateTime ReservedAt { get; }
}