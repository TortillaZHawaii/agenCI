namespace Agenci.Abstractions;

public interface IDriverUserGrain : IGrainWithStringKey
{
    public Task<List<ParkingOffer>> GetParkingOffers(double latitude, double longitude, DateTime start, DateTime end);
   
    public Task<bool> ChooseParking(string parkingId);
    
    public Task<List<ParkingOffer>> GetReservedParkingHistory();
}
