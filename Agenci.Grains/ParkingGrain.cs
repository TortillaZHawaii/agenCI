using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class ParkingGrain : IParkingGrain, IParkingUserGrain
{
    private readonly IPersistentState<List<Reservation>> _reservations;
    private readonly IPersistentState<ParkingInfo> _parkingInfo;
    
    public ParkingGrain(
        [PersistentState("parkingIntervals", "parkingIntervalsStore")]
        IPersistentState<List<Reservation>> reservations,
        [PersistentState("parkingDto", "parkingDtoStore")]
        IPersistentState<ParkingInfo> parkingInfo)
    {
        _reservations = reservations;
        _parkingInfo = parkingInfo;
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
        throw new NotImplementedException();
    }

    public Task UpdateParkingInfo(ParkingInfo parkingInfo)
    {
        throw new NotImplementedException();
    }
}
