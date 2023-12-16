using Agenci.Abstractions;
using Orleans.Runtime;

namespace Agenci.Grains;

public class ParkingGrain : Grain, IParkingGrain, IParkingUserGrain
{
    private readonly IPersistentState<List<Reservation>> _reservations;
    private readonly IPersistentState<ParkingInfo> _parkingInfo;
    private readonly IGrainFactory _grainFactory;
    
    public ParkingGrain(
        [PersistentState("parkingReservations", "parkingStore")]
        IPersistentState<List<Reservation>> reservations,
        [PersistentState("parkingInfo", "parkingStore")]
        IPersistentState<ParkingInfo> parkingInfo,
        IGrainFactory grainFactory)
    {
        _reservations = reservations;
        _parkingInfo = parkingInfo;
        _grainFactory = grainFactory;
    }
    
    private List<int> GetAvailableSpots(DateTime start, DateTime end)
    {
        var blocked = _reservations.State
            .Where(reservation => reservation.Start < end || reservation.End > start)
            .Select(reservation => reservation.PlaceNumber)
            .ToHashSet();

        var availableSpots = new List<int>();
        
        for (int spot = 0; spot < _parkingInfo.State.MaxCapacity; ++spot)
        {
            if (!blocked.Contains(spot))
            {
                availableSpots.Add(spot);
            }
        }
        
        return availableSpots;
    }
    
    private decimal GetPrice(DateTime start, DateTime end, int freeSpots)
    {
        // some pseudo random price
        var pricePerHour = this.GetHashCode() % 10 + 1;
        var capacityFactorLowerBound = 0.5;
        var capacityFactor = Math.Max(
            1 - (double)freeSpots / _parkingInfo.State.MaxCapacity,
            capacityFactorLowerBound);
        var price = (decimal)((end - start).TotalHours * pricePerHour * capacityFactor);
        return price;
    }
    
    public async Task<ParkingOffer> GetParkingPriceResponse(DateTime start, DateTime end, string driverKey)
    {
        var availableSpots = GetAvailableSpots(start, end);

        if (availableSpots.Count == 0)
        {
            return new ParkingOffer
            {
                Key = this.GetPrimaryKeyString(),
                Latitude = _parkingInfo.State.Latitude,
                Longitude = _parkingInfo.State.Longitude,
                Address = _parkingInfo.State.Address,
                MaxCapacity = _parkingInfo.State.MaxCapacity,
                Price = null,
                Start = null,
                End = null,
            };
        }
        
        // reserve first available spot
        var reservation = new Reservation
        {
            DriverKey = driverKey,
            Price = GetPrice(start, end, availableSpots.Count),
            Start = start,
            End = end,
            Paid = false,
            PlaceNumber = availableSpots[0],
        };
        
        _reservations.State.Add(reservation);
        await _reservations.WriteStateAsync();
        
        return new ParkingOffer
        {
            Key = this.GetPrimaryKeyString(),
            Latitude = _parkingInfo.State.Latitude,
            Longitude = _parkingInfo.State.Longitude,
            Address = _parkingInfo.State.Address,
            MaxCapacity = _parkingInfo.State.MaxCapacity,
            Price = reservation.Price,
            Start = reservation.Start,
            End = reservation.End,
        };
    }

    
    /// <param name="driverKey"></param>
    /// <returns>Place number or null if nothing is available</returns>
    public async Task<int?> ReserveParking(string driverKey)
    {
        var reservation = _reservations.State
            .Where(reservation => reservation.Paid == false)
            .FirstOrDefault(reservation => reservation.DriverKey == driverKey);
        
        if (reservation is null)
        {
            return null;
        }

        _reservations.State.Remove(reservation);
        _reservations.State.Add(reservation with { Paid = true });
        await _reservations.WriteStateAsync();
        
        return reservation.PlaceNumber;
    }

    public async Task CancelReservation(string driverKey)
    {
        var reservation = _reservations.State
            .Where(reservation => reservation.Paid == false)
            .FirstOrDefault(reservation => reservation.DriverKey == driverKey);

        if (reservation is not null)
        {
            _reservations.State.Remove(reservation);
            await _reservations.WriteStateAsync();
        }
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
