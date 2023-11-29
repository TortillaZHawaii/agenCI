namespace Agenci.Abstractions;

/// <summary>
/// Detailed information about a parking. Send by parking.
/// </summary>
[GenerateSerializer, Immutable]
public record ParkingDetailedInfo
{
    /// <summary>
    /// An address for grains communication as well as unique name.
    /// </summary>
    [Id(0)]
    public required string Key { get; init; }

    /// <summary>
    /// Latitude of the parking.
    /// </summary>
    [Id(1)]
    public required double Latitude { get; init; }

    /// <summary>
    /// Longitude of the parking.
    /// </summary>
    [Id(2)]
    public required double Longitude { get; init; }

    /// <summary>
    /// Some string formatted to look like an address.
    /// </summary>
    [Id(3)]
    public required string Address { get; init; }

    /// <summary>
    /// Maximum capacity that the parking can hold at any moment.
    /// </summary>
    [Id(4)]
    public required int MaxCapacity { get; init; }
    
    /// <summary>
    /// List of reservations for this parking.
    /// </summary>
    [Id(5)]
    public required List<Reservation> Reservations { get; init; }
}