using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class OrchestratorGrain : Grain, IOrchestratorGrain
{
    // https://learn.microsoft.com/en-us/dotnet/orleans/grains/grain-persistence/?pivots=orleans-7-0
    private readonly IPersistentState<List<ParkingInfo>> _state;
    
    public OrchestratorGrain(
        [PersistentState("orchestrator", "orchestratorStore")]
        IPersistentState<List<ParkingInfo>> state)
    {
        _state = state;
    }
    
    public async Task ReceiveParkingInformation(ParkingInfo parkingInfo)
    {
        _state.State.Add(parkingInfo);
        await _state.WriteStateAsync();
    }

    public Task<ParkingsInRange> GetParkingsInRange(double latitude, double longitude)
    {
        var parkings = _state.State
            .Where(parking => GetDistanceSquared(latitude, longitude, parking.Latitude, parking.Longitude) < 0.1)
            .ToList();
        return Task.FromResult(new ParkingsInRange
        {
            Latitude = latitude,
            Longitude = longitude,
            Parkings = parkings
        });
    }

    private double GetDistanceSquared(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        var latitudeDifference = latitude1 - latitude2;
        var longitudeDifference = longitude1 - longitude2;
        return latitudeDifference * latitudeDifference + longitudeDifference * longitudeDifference;
    }
}
