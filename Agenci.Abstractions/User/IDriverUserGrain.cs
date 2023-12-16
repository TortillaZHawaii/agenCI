namespace Agenci.Abstractions;

public interface IDriverUserGrain : IGrainWithStringKey
{
    public Task<List<ParkingOffer>> GetParkingOffers(double latitude, double longitude, DateTime start, DateTime end);
   
    /// <returns>Place ID within the parking or null if not available anymore</returns>
    public Task<int?> ChooseParking(string parkingId);
    
    public Task<List<ParkingOffer>> GetReservedParkingHistory();
}
