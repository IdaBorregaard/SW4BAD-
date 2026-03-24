using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;
using assignment3.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrering af Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Her forbinder vi din DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AarhusSpaceContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapOpenApi("/openapi/v1.json");

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


// Query endpoints


app.Run();