using CSharpFunctionalExtensions;
using SeatReservation.Domain.Events;
using SeatReservation.Domain.Shared;

namespace SeatReservation.Domain.Venues;

/// <summary>
/// Класс площадки
/// </summary>
public class Venue
{
    /// <summary>
    /// Навигационная связь с таблицей Seat
    /// </summary>
    private List<Seat> _seats = [];

    /// <summary>
    /// Навигационная связь с таблицей Event
    /// </summary>
    private List<Event> _events = [];

    public Venue(Guid id, string name, int seatsLimit, IEnumerable<Seat> seats)
    {
        Id = id;
        Name = name;
        SeatsLimit = seatsLimit;
        _seats = seats.ToList();
    }

    /// <summary>
    /// id площадки
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Имя площадки
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Максимальное количество сидений на площадке
    /// </summary>
    public int SeatsLimit { get; private set; }

    /// <summary>
    /// Максимальное количество сидений. Расчитывается динамически из свойства _seats
    /// </summary>
    public int SeatsCount => _seats.Count;

    /// <summary>
    /// Приватная коллекция Seat. Нужна чтобы извне нельзя было её изменить
    /// </summary>
    public IReadOnlyList<Seat> Seats => _seats;

    /// <summary>
    /// Приватная коллекция Event. Нужна чтобы извне нельзя было её изменить
    /// </summary>
    public IReadOnlyList<Event> Events => _events;

    /// <summary>
    /// Метод для добавления нового сидения
    /// </summary>
    public UnitResult<Error> AddSeat(Seat seat)
    {
        if (SeatsCount >= SeatsLimit)
        {
            return Error.Conflict("venue.seats.limit", "");
        }

        if (_seats.Any(s => s.RowNumber == seat.RowNumber && s.SeatNumber == seat.SeatNumber))
        {
            return Error.Conflict("venue.seat.duplicate", "Место с такими параметрами уже существует");
        }


        _seats.Add(seat);

        return UnitResult.Success<Error>();
    }

    /// <summary>
    /// Метод для расширения максимального колличества сидений
    /// </summary>
    public void ExpandSeatsLimit(int newSeatsLimit) => SeatsLimit = newSeatsLimit;
}