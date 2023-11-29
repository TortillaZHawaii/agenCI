namespace Agenci.Abstractions;

public interface IOrchestratorGrain : IGrainWithGuidKey
{
    public Task ReceiveParkingInformation(ParkingInfo parkingInfo);
    public Task<ParkingsInRange> GetParkingsInRange(double latitude, double longitude);
}
