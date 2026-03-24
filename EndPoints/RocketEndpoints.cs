using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// ROCKET CRUD ENDPOINTS
// ==========================================

public static class RocketEndpoints
{
    public static void MapRocketEndpoints(this IEndpointRouteBuilder app)
    {

        // Create a new rocket
        app.MapPost("/api/rockets", async (RocketCreateDTO createDTO, AarhusSpaceContext db)
        =>
        {
            if (createDTO.Weight < 0)
            {
                return Results.BadRequest("Weight cannot be negative.");
            }

            // Map the incoming DTO to a new Rocket entity
            var newRocket = new Rocket
            {
                RocketId = createDTO.RocketId,
                Name = createDTO.Name,
                Weight = createDTO.Weight,
                FuelCap = createDTO.FuelCap,
                Payload = createDTO.Payload,
                Stages = createDTO.Stages,
                CrewCap = createDTO.CrewCap
            };

            db.Rockets.Add(newRocket);
            await db.SaveChangesAsync();

            // Return the created rocket as a DTO in the response
            var response = new RocketDTO
            {
                RocketId = newRocket.RocketId,
                Name = newRocket.Name,
                Weight = newRocket.Weight,
                FuelCap = newRocket.FuelCap,
                Payload = newRocket.Payload,
                Stages = newRocket.Stages,
                CrewCap = newRocket.CrewCap
            };

            return Results.Created($"/api/rockets/{response.RocketId}", response);
        });

        // Get all rockets
        app.MapGet("/api/rockets", async (AarhusSpaceContext db) =>
        {
            var rockets = await db.Rockets
            .Select(r => new RocketDTO
            {
                RocketId = r.RocketId,
                Name = r.Name,
                Weight = r.Weight,
                FuelCap = r.FuelCap,
                Payload = r.Payload,
                Stages = r.Stages,
                CrewCap = r.CrewCap
            }).ToListAsync();

            return Results.Ok(rockets);
        });

        // Get a rocket by ID
        app.MapGet("/api/rockets/{id}", async (string id, AarhusSpaceContext db) =>
        {
            var rocket = await db.Rockets
            .Where(r => r.RocketId == id)
            .Select(r => new RocketDTO
            {
                RocketId = r.RocketId,
                Name = r.Name,
                Weight = r.Weight,
                FuelCap = r.FuelCap,
                Payload = r.Payload,
                Stages = r.Stages,
                CrewCap = r.CrewCap
            }
            ).FirstOrDefaultAsync();

            return rocket is not null ? Results.Ok(rocket) : Results.NotFound();
        });

        // Update existing rocket
        app.MapPut("/api/rockets/{id}", async (string id, RocketUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var rocket = await db.Rockets
            .FirstOrDefaultAsync(r => r.RocketId == id);

            if (rocket is null)
            {
                return Results.NotFound($"Could not find rocket with ID {id}!");
            }

            if (updateDTO.Weight < 0)
            {
                return Results.BadRequest("Weight cannot be negative.");
            }

            rocket.Name = updateDTO.Name;
            rocket.Weight = updateDTO.Weight;
            rocket.FuelCap = updateDTO.FuelCap;
            rocket.Payload = updateDTO.Payload;
            rocket.Stages = updateDTO.Stages;
            rocket.CrewCap = updateDTO.CrewCap;
            
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete rocket
        app.MapDelete("/api/rockets/{id}", async (string id, AarhusSpaceContext db) =>
        {
            var rocket = await db.Rockets.FindAsync(id);
            if (rocket is null)
            {
                return Results.NotFound($"Could not find rocket with ID {id}!");
            }

            db.Rockets.Remove(rocket);
            await db.SaveChangesAsync();
            return Results.Ok($"Rocket with ID {id} has been removed!");
        });


    }
}