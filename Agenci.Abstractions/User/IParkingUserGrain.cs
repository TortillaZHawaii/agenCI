namespace Agenci.Abstractions;

public interface IParkingUserGrain : IGrainWithStringKey
{
    public Task<ParkingDetailedInfo> GetParkingDetailedInfo();
    
    public Task UpdateParkingInfo(ParkingInfo parkingInfo);
}
