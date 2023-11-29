namespace Agenci.Abstractions;

public interface IParkingGrain : IGrainWithStringKey
{
    public Task<ParkingOffer> GetParkingPriceResponse(DateTime start, DateTime end, string driverKey);
    
    public Task<bool> ReserveParking(string driverKey);
}
