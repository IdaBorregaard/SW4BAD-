using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;
using assignment3.Endpoints;
using Scalar.AspNetCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// The ?? operator says: "If the config is missing, use this default string instead."
var mongoDbConnection = builder.Configuration.GetConnectionString("MongoLogConnection") 
    ?? "mongodb://localhost:27017/SpaceMissionLogs";

// Configure Serilog to write to MongoDB
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // Tell .NET to use Serilog instead of the default logger

// 1. Registrering af Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Her forbinder vi din DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AarhusSpaceContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapOpenApi("/openapi/v1.json");

if (app.Environment.IsDevelopment())
{
    // In Scalar v2.0+, pass the URL string directly!
    app.MapScalarApiReference("/scalar");
}

// CRUD endpoints for every entity

// CRUD endpoints for managers
app.MapManagerEndpoints();

// CRUD endpoints for astronauts
app.MapAstronautEndpoints();

// CRUD endpoints for scientists
app.MapScientistEndpoints();

// CRUD endpoints for rockets
app.MapRocketEndpoints();

// CRUD endpoints for launchpads
app.MapLaunchpadEndpoints();

// CRUD endpoints for missions
app.MapMissionsEndpoints();

// CRUD endpoints for celestial bodies
app.MapBodiesEndpoints();


// Custom Middleware to log POST, PUT, DELETE requests
app.Use(async (context, next) =>
{
    // Let the request happen first so we can get the Response Status Code
    await next(context);

    var method = context.Request.Method;

    // Only log if it is a POST, PUT, or DELETE
    if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Delete)
    {
        Log.Information("API Request Logged | Method: {Method} | Path: {Path} | Status: {StatusCode} | Timestamp: {Timestamp}",
            method,
            context.Request.Path,
            context.Response.StatusCode,
            DateTime.UtcNow);
    }
});


app.Run();