using Agenci.Abstractions;
using Orleans.TestingHost;
using Xunit;

namespace Agenci.UnitTests;

[Collection(ClusterCollection.Name)]
public class ReserveParkingTests(ClusterFixture fixture)
{
    private readonly TestCluster _cluster = fixture.Cluster;
    
    [Fact]
    public async Task ReserveParkingTest()
    {
        var orchestrator = _cluster.GrainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
        var parking = _cluster.GrainFactory.GetGrain<IParkingUserGrain>("Test parking");
        var driver = _cluster.GrainFactory.GetGrain<IDriverUserGrain>("Test driver");
        
        // Create parking
        await parking.UpdateParkingInfo(new ParkingInfo
        {
            Key = "Test parking",
            Address = "Test address",
            Latitude = 0,
            Longitude = 0,
            MaxCapacity = 1
        });
        
        // Check if parking has been added to orchestrator
        var parkingsInRange = await orchestrator.GetParkingsInRange(0, 0);
        Assert.Single(parkingsInRange.Parkings);
        
        // Get offers for driver
        var offers = await driver.GetParkingOffers(0, 0, 
            DateTime.Now, DateTime.Now.AddHours(2));
        
        // Check if there is an offer
        Assert.Single(offers);

        // Choose parking
        var parkingNumber = await driver.ChooseParking("Test parking");
        
        // Check if parking has been reserved for driver
        Assert.NotNull(parkingNumber);
        var history = await driver.GetReservedParkingHistory();
        Assert.Single(history);
        Assert.Equal(history[0].Key, parking.GetPrimaryKeyString());
        
        // Check if parking has been reserved for parking
        var parkingInfo = await parking.GetParkingDetailedInfo();
        Assert.Single(parkingInfo.Reservations);
        Assert.Equal(parkingInfo.Reservations[0].PlaceNumber, parkingNumber);
        Assert.Equal(parkingInfo.Reservations[0].DriverKey, driver.GetPrimaryKeyString());
    }
}