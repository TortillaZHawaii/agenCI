using Agenci.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Default", policyBuilder => policyBuilder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("orchestratorStore");
    siloBuilder.AddMemoryGrainStorage("driverStore");
    siloBuilder.AddMemoryGrainStorage("parkingStore");
    siloBuilder.AddMemoryGrainStorageAsDefault();
    // siloBuilder.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory());
    siloBuilder.UseDashboard(x => x.HostSelf = true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("Default");

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

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

app.MapPost("/parkings/{id}", async (string id, ParkingInfo info, IGrainFactory grainFactory) =>
    {
        var parkingGrain = grainFactory.GetGrain<IParkingUserGrain>(id);
        await parkingGrain.UpdateParkingInfo(info);
    })
    .WithName("UpdateParkingInfo")
    .WithOpenApi();

app.MapGet("/drivers/{id}/history", (string id, IGrainFactory grainFactory) =>
    {
        var driverGrain = grainFactory.GetGrain<IDriverUserGrain>(id);
        return driverGrain.GetReservedParkingHistory();
    })
    .WithName("GetDriverHistory")
    .WithOpenApi();

// offers for driver in given location
app.MapGet("/drivers/{id}/parkings", (string id, double latitude, double longitude, DateTime start, DateTime end, IGrainFactory grainFactory) =>
    {
        var driverGrain = grainFactory.GetGrain<IDriverUserGrain>(id);
        return driverGrain.GetParkingOffers(latitude, longitude, start, end);
    })
    .WithName("GetParkingOffers")
    .WithOpenApi();

app.MapPost("/parkings/{id}/reserve", async (string id, string driverId, IGrainFactory grainFactory) =>
    {
        var driverGrain = grainFactory.GetGrain<IDriverUserGrain>(driverId);
        var result = await driverGrain.ChooseParking(id);
        if (result is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    })
    .WithName("ReserveParking")
    .WithOpenApi();

app.Map("/dashboard", c => c.UseOrleansDashboard());

app.Run();
