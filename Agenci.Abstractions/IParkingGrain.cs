using Orleans;

namespace Agenci.Abstractions;

public interface IParkingGrain : IGrainWithStringKey
{
    public Task<decimal?> GetParkingPrice(DateTime start, DateTime end);
    public Task<bool> ReserveParking(DateTime start, DateTime end, );
}

