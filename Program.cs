using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;

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

// add queries 

app.Run();