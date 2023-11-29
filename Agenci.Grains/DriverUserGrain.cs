using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class DriverUserGrain : IDriverUserGrain
{
    private readonly IGrainFactory _grainFactory;
    private readonly IPersistentState<List<ParkingOffer>> _parkingOffers;
    private readonly IPersistentState<List<ParkingOffer>> _history;

    public DriverUserGrain(
        IGrainFactory grainFactory,
        [PersistentState("parkingOffers", "driverStore")]
        IPersistentState<List<ParkingOffer>> parkingOffers,
        [PersistentState("parkingOffers", "driverStore")]
        IPersistentState<List<ParkingOffer>> history)
    {
        _grainFactory = grainFactory;
        _parkingOffers = parkingOffers;
        _history = history;
    }
    
    public async Task<List<ParkingOffer>> GetParkingOffers(double latitude, double longitude, DateTime start, DateTime end)
    {
        var orchestratorGrain = _grainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
        var parkingsInRange = await orchestratorGrain.GetParkingsInRange(latitude, longitude);
        var parkingGrains = parkingsInRange.Parkings
            .Select(parking => _grainFactory.GetGrain<IParkingGrain>(parking.Key));
        
        var parkingOffers = await Task.WhenAll(
            parkingGrains.Select(parkingGrain
                => parkingGrain.GetParkingPriceResponse(start, end, this.GetPrimaryKeyString())));
        
        _parkingOffers.State = parkingOffers.ToList();
        return parkingOffers.Where(offer => offer.IsAvailable).ToList();
    }

    public async Task<bool> ChooseParking(string parkingId)
    {
        if (_parkingOffers.State.All(offer => offer.Key != parkingId))
        {
            return false;
        }
        
        var parkingGrain = _grainFactory.GetGrain<IParkingGrain>(parkingId);
        
        var result = await parkingGrain.ReserveParking(this.GetPrimaryKeyString());
        
        if (result) 
        {
            _history.State.Add(_parkingOffers.State.First(offer => offer.Key == parkingId));
            _parkingOffers.State.RemoveAll(offer => true);
            await _history.WriteStateAsync();
            await _parkingOffers.WriteStateAsync();
            return true;
        }
        
        return false;
    }

    public Task<List<ParkingOffer>> GetReservedParkingHistory()
    {
        return Task.FromResult(_history.State);
    }
}