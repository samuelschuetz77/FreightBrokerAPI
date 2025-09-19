using Microsoft.EntityFrameworkCore;
using FreightBrokerAPI.Data;


var builder = WebApplication.CreateBuilder(args);

// Prefer appsettings.json, but fall back to a local file if missing
var connString = builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? "Data Source=freight.db";

// We'll create FreightDbContext in the next step
builder.Services.AddDbContext<FreightDbContext>(opt =>
    opt.UseSqlite(connString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


// tiny health probe so you know the app is alive
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
.WithTags("system")
   .Produces(200)
   .WithSummary("Simple health probe")
   .WithDescription("Returns a JSON object { status = 'ok' } to confirm the API is alive");
   

app.Run();

