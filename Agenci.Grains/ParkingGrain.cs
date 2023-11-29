using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class ParkingGrain : IParkingGrain
{
    private readonly IPersistentState<List<ParkingInterval>> _parkingIntervals;
    private readonly IPersistentState<ParkingInfo> _parkingInfo;
    
    public ParkingGrain(
        [PersistentState("parkingIntervals", "parkingIntervalsStore")]
        IPersistentState<List<ParkingInterval>> parkingIntervals,
        [PersistentState("parkingDto", "parkingDtoStore")]
        IPersistentState<ParkingInfo> parkingInfo)
    {
        _parkingIntervals = parkingIntervals;
        _parkingInfo = parkingInfo;
    }
    
    public Task<decimal?> GetParkingPrice(DateTime start, DateTime end)
    {
        int maxCapacity = _parkingInfo.State.MaxCapacity;
        int currentCapacity = _parkingIntervals.State
            .Count(interval => interval.Start < end && interval.End > start);
        
        if (currentCapacity < maxCapacity)
        {
            return Task.FromResult<decimal?>(15.0m);
        }
        
        return Task.FromResult<decimal?>(null);
    }
}

[GenerateSerializer, Immutable]
public record ParkingInterval
{
    [Id(0)]
    public required DateTime Start { get; init; }
    
    [Id(1)]
    public required DateTime End { get; init; }
    
    [Id(2)]
    public required decimal Price { get; init; }
    
    [Id(3)]
    public required string DriverKey { get; init; }
}
