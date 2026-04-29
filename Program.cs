using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;
using assignment3.Endpoints;
using Scalar.AspNetCore;
using Microsoft.Extensions.Options;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

// Identity services: AddIdentity for handling user management, and specify that we want to use our AppUser class and IdentityRole for roles
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AarhusSpaceContext>()
    .AddDefaultTokenProviders();

// JWT Authentication setup
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"]!; // The "!" tells the compiler that we are sure this value will not be null, since we have a default in appsettings.json

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

builder.Services.AddAuthorization(); // This is needed to use the [Authorize] attribute in endpoints

var app = builder.Build();

app.MapOpenApi("/openapi/v1.json");

if (app.Environment.IsDevelopment())
{
    // In Scalar v2.0+, pass the URL string directly!
    app.MapScalarApiReference("/scalar");
}

// CRUD endpoints for every entity

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// CRUD endpoints for login
app.MapAuthEndpoints();

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

// CRUD endpoints for Experiments
app.MapExperimentEndpoints();


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

// Seed users and roles
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedUsers.SeedAsync(userManager, roleManager);
}

app.Run();