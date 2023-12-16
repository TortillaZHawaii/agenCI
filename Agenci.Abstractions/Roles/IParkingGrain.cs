namespace Agenci.Abstractions;

public interface IParkingGrain : IGrainWithStringKey
{
    public Task<ParkingOffer> GetParkingPriceResponse(DateTime start, DateTime end, string driverKey);
    
    public Task<int?> ReserveParking(string driverKey);
    
    public Task CancelReservation(string driverKey);
}
