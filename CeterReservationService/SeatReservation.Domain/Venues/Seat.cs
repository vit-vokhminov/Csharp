using CSharpFunctionalExtensions;
using SeatReservation.Domain.Shared;

namespace SeatReservation.Domain.Venues;

/// <summary>
/// Класс места
/// </summary>
public class Seat
{
    public Seat(Guid id, int rowNumber, int seatNumber)
    {
        Id = id;
        RowNumber = rowNumber;
        SeatNumber = seatNumber;
    }

    /// <summary>
    /// id места
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Ряд места
    /// </summary>
    public int RowNumber { get; private set; }

    /// <summary>
    /// Номер места
    /// </summary>
    public int SeatNumber { get; private set; }

    /// <summary>
    /// Метод для создания экземпляра класса Seat
    /// </summary>
    public static Result<Seat, Error> Create(int rowNumber, int seatNumber)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
        {
            return Error.Validation("seat.rowNumber", "Номер ряда и количество посадочных мест должны быть больше нуля");
        }

        return new Seat(Guid.NewGuid(), rowNumber, seatNumber);
    }
}