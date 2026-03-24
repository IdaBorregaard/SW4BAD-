using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// LAUNCHPAD CRUD ENDPOINTS
// ==========================================
public static class LaunchpadEndpoints
{
    public static void MapLaunchpadEndpoints(this IEndpointRouteBuilder app)
    {   

        // Create a new launch pad
        app.MapPost("/api/launchpads", async (LaunchPadCreateDTO createDTO, AarhusSpaceContext db)
        =>{
            
            var newLaunchPad = new LaunchPad
            {
                LocationId = createDTO.LocationId,
                Status = createDTO.Status,
                MaxWeight = createDTO.MaxWeight
            };

            db.LaunchPads.Add(newLaunchPad);
            await db.SaveChangesAsync();

            // Return the created launch pad as a DTO in the response
            var response = new LaunchPadDTO
            {
                LocationId = newLaunchPad.LocationId,
                Status = newLaunchPad.Status,
                MaxWeight = newLaunchPad.MaxWeight
            };

            return Results.Created($"/api/launchpads/{response.LocationId}", response);
        });

        // Get all launch pads
        app.MapGet("/api/launchpads", async (AarhusSpaceContext db) =>
        {
            var launchPads = await db.LaunchPads
            .Select(l => new LaunchPadDTO
            {
                LocationId = l.LocationId,
                Status = l.Status,
                MaxWeight = l.MaxWeight
            }).ToListAsync();

            return Results.Ok(launchPads);
        });

        // Get a launch pad by ID
        app.MapGet("/api/launchpads/{id}", async (string id, AarhusSpaceContext db) =>
        {
            var launchPad = await db.LaunchPads
            .Where(l => l.LocationId == id)
            .Select(l => new LaunchPadDTO
            {
                LocationId = l.LocationId,
                Status = l.Status,
                MaxWeight = l.MaxWeight
            }
            ).FirstOrDefaultAsync();

            return launchPad is not null ? Results.Ok(launchPad) : Results.NotFound();
        });

        // Update existing launch pad
        app.MapPut("/api/launchpads/{id}", async (string id, LaunchPadUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var launchPad = await db.LaunchPads
            .FirstOrDefaultAsync(l => l.LocationId == id);
            if (launchPad is null)
            {
                return Results.NotFound($"Could not find launch pad with ID {id}!");
            }

            launchPad.Status = updateDTO.Status;
            launchPad.MaxWeight = updateDTO.MaxWeight;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete launch pad
        app.MapDelete("/api/launchpads/{id}", async (string id, AarhusSpaceContext db) =>
        {
            var launchPad = await db.LaunchPads.FindAsync(id);
            if (launchPad is null)    {
                return Results.NotFound($"Could not find launch pad with ID {id}!");
            } 
            db.LaunchPads.Remove(launchPad);
            await db.SaveChangesAsync();
            return Results.Ok($"Launch pad with ID {id} has been removed!");
        });
    }
}