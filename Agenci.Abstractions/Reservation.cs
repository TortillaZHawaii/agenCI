namespace Agenci.Abstractions;

[GenerateSerializer, Immutable]
public record Reservation
{
    [Id(0)]
    public required string DriverKey { get; init; }
    
    [Id(1)]
    public required decimal Price { get; init; }
    
    [Id(2)]
    public required DateTime Start { get; init; }

    [Id(3)]
    public required DateTime End { get; init; }
}