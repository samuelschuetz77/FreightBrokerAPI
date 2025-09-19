using Microsoft.EntityFrameworkCore;
using FreightBrokerAPI.Data;
using FreightBrokerAPI.Models;
using FreightBrokerAPI.Migrations;



var builder = WebApplication.CreateBuilder(args);

// Prefer appsettings.json, but fall back to a local file if missing
var connString = builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? "Data Source=freight.db";

// We'll create FreightDbContext in the next step
builder.Services.AddDbContext<FreightDbContext>(opt =>
    opt.UseSqlite(connString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JSON serialization to handle circular references
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FreightDbContext>();
    db.Database.EnsureCreated(); // makes sure DB exists
    DbInitializer.Seed(db);
}



// tiny health probe so you know the app is alive
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
.WithTags("system")
   .Produces(200)
   .WithSummary("Simple health probe")
   .WithDescription("Returns a JSON object { status = 'ok' } to confirm the API is alive");

app.MapGet("/loads", async (FreightDbContext db) =>
    await db.Loads
        .Include(l => l.Shipper)
        .Select(l => new LoadDto(
            l.LoadId,
            l.Origin,
            l.Destination,
            l.ShipperRate,
            l.Shipper!.Name,
            l.Shipper!.ShipperId
        ))
        .ToListAsync())
        .WithTags("loads")
        .Produces<List<Load>>(200)
        .WithSummary("Get all loads")
        .WithDescription("Fetches all posted loads from the database:");
   

app.MapPost("/loads", async (FreightDbContext db, Load load) =>
{
    db.Loads.Add(load);
    await db.SaveChangesAsync();
    return Results.Created($"/loads/{load.LoadId}", load);
})
.WithTags("loads")
.Produces<Load>(201)
.WithSummary("Create a new load")
.WithDescription("Adds a new load to the database and returns it with the assigned LoadId");


   
app.Run();

public record LoadDto(
    int LoadId,
    string Origin,
    string Destination,
    decimal ShipperRate,
    string ShipperName,
    int ShipperId
);
