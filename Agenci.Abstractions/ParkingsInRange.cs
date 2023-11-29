using Orleans;

namespace Agenci.Abstractions;

/// <summary>
/// List of parkings in range of a driver from a given location.
/// </summary>
[GenerateSerializer, Immutable]
public record ParkingsInRange
{
    [Id(0)]
    public required double Latitude { get; init; }
    
    [Id(1)]
    public required double Longitude { get; init; }
    
    [Id(2)]
    public required List<ParkingInfo> Parkings { get; init; }
}
