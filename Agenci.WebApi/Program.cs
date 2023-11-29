using Agenci.Abstractions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("orchestratorStore");
    siloBuilder.AddMemoryGrainStorageAsDefault();
    // siloBuilder.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory());
    siloBuilder.UseDashboard(x => x.HostSelf = true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/parkings", (double latitude, double longitude, IGrainFactory grainFactory) =>
    {
        var orchestratorGrain = grainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
        return orchestratorGrain.GetParkingsInRange(latitude, longitude);
    })
    .WithName("GetParkingsInRange")
    .WithOpenApi();

app.MapGet("/parkings/{id}", (string id, IGrainFactory grainFactory) =>
    {
        var parkingGrain = grainFactory.GetGrain<IParkingUserGrain>(id);
        return parkingGrain.GetParkingDetailedInfo();
    })
    .WithName("GetParkingInfo")
    .WithOpenApi();

app.MapPost("/parkings/{id}", (string id, ParkingInfo info, IGrainFactory grainFactory) =>
    {
        var parkingGrain = grainFactory.GetGrain<IParkingUserGrain>(id);
        parkingGrain.UpdateParkingInfo(info);
    })
    .WithName("ReserveParking")
    .WithOpenApi();

app.MapPost("/parkings", async (ParkingInfo parking, IGrainFactory grainFactory) =>
    {
        var orchestratorGrain = grainFactory.GetGrain<IOrchestratorGrain>(Guid.Empty);
        await orchestratorGrain.ReceiveParkingInformation(parking);
    })
    .WithName("AddParking")
    .WithOpenApi();

app.Map("/dashboard", c => c.UseOrleansDashboard());

app.Run();
