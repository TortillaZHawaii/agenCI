using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class ParkingGrain : IParkingGrain, IParkingUserGrain
{
    private readonly IPersistentState<List<Reservation>> _reservations;
    private readonly IPersistentState<ParkingInfo> _parkingInfo;
    private readonly IGrainFactory _grainFactory;
    
    public ParkingGrain(
        [PersistentState("parkingIntervals", "parkingIntervalsStore")]
        IPersistentState<List<Reservation>> reservations,
        [PersistentState("parkingDto", "parkingDtoStore")]
        IPersistentState<ParkingInfo> parkingInfo,
        IGrainFactory grainFactory)
    {
        _reservations = reservations;
        _parkingInfo = parkingInfo;
        _grainFactory = grainFactory;
    }
    
    public Task<ParkingOffer> GetParkingPriceResponse(DateTime start, DateTime end, string driverKey)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReserveParking(string driverKey)
    {
        throw new NotImplementedException();
    }

    public Task<ParkingDetailedInfo> GetParkingDetailedInfo()
    {
        return Task.FromResult(
            new ParkingDetailedInfo
            {
                Key = _parkingInfo.State.Key,
                Address = _parkingInfo.State.Address,
                Latitude = _parkingInfo.State.Latitude,
                Longitude = _parkingInfo.State.Longitude,
                MaxCapacity = _parkingInfo.State.MaxCapacity,
                Reservations = _reservations.State,
            }
        );
    }

    public async Task UpdateParkingInfo(ParkingInfo parkingInfo)
    {
        if (parkingInfo.Key != this.GetPrimaryKeyString())
        {
            throw new ArgumentException("ParkingInfo key must be equal to this grain key");
        }
        _parkingInfo.State = parkingInfo;
        await _parkingInfo.WriteStateAsync();
        var orchestratorGrain = _grainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
        await orchestratorGrain.ReceiveParkingInformation(parkingInfo);
    }
}
