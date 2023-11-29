using Agenci.Abstractions;
using ParkingDemo.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("orchestratorStore");
    siloBuilder.AddMemoryGrainStorageAsDefault();
    // siloBuilder.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory());
    siloBuilder.UseDashboard(x => x.HostSelf = true);
});

var app = builder.Build();

// // // Add parking info to orchestrator
// var grainFactory = app.Services.GetRequiredService<IGrainFactory>();
// var orchestratorGrain = grainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
//
// if (orchestratorGrain == null)
// {
//     Console.WriteLine("Orchestrator grain is null");
//     return;
// }
// await orchestratorGrain.ReceiveParkingInformation(new ParkingInfo
// {
//     Latitude = 0,
//     Longitude = 0,
//     Address = "Some random address",
//     Key = "Some random key",
//     MaxCapacity = 123
// });
//
// var parkings = await orchestratorGrain.GetParkingsInRange(0, 0);
// Console.WriteLine(parkings);

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Map("/dashboard", x => x.UseOrleansDashboard());
app.Run();