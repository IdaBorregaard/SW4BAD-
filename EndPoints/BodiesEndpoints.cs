using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// BODIES CRUD ENDPOINTS
// ==========================================

public static class BodiesEndpoints
{
    public static void MapBodiesEndpoints(this IEndpointRouteBuilder app)
    {
        
        // Create a new celestial body
        app.MapPost("/api/bodies", [Authorize(Roles = "Manager")] async (CelestialBodyCreateDTO createDTO, AarhusSpaceContext db)
        =>{
            var newBody = new CelestialBody
            {
                Name = createDTO.Name,
                Dist = createDTO.Dist,
                BodyType = createDTO.BodyType,
                ParentPlanetName = createDTO.ParentPlanetName
            };
            db.Bodies.Add(newBody);
            await db.SaveChangesAsync();

            var response = new CelestialBodyDTO
            {
                Name = newBody.Name,
                Dist = newBody.Dist,
                BodyType = newBody.BodyType,
                ParentPlanetName = newBody.ParentPlanetName
            };

            return Results.Created($"/api/bodies/{response.Name}", response);
        });

        // Get all celestial bodies
        app.MapGet("/api/bodies", [Authorize] async (AarhusSpaceContext db) =>
        {
            var bodies = await db.Bodies
            .Select(b => new CelestialBodyDTO
            {
                Name = b.Name,
                Dist = b.Dist,
                BodyType = b.BodyType,
                ParentPlanetName = b.ParentPlanetName
            }).ToListAsync();

            return Results.Ok(bodies);
        });

        // Get a celestial body by ID
        app.MapGet("/api/bodies/{id}", [Authorize] async (string id, AarhusSpaceContext db) =>
        {
            var body = await db.Bodies
            .Where(b => b.Name == id)
            .Select(b => new CelestialBodyDTO
            {
                Name = b.Name,
                Dist = b.Dist,
                BodyType = b.BodyType,
                ParentPlanetName = b.ParentPlanetName
            }
            ).FirstOrDefaultAsync();

            return body is not null ? Results.Ok(body) : Results.NotFound();
        });

        // Update existing celestial body
        app.MapPut("/api/bodies/{id}", [Authorize(Roles = "Manager")] async (string id, CelestialBodyUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var body = await db.Bodies
            .FirstOrDefaultAsync(b => b.Name == id);

            if (body is null)
            {
                return Results.NotFound($"Could not find celestial body with name {id}!");
            }

            body.Dist = updateDTO.Dist;
            body.BodyType = updateDTO.BodyType;
            body.ParentPlanetName = updateDTO.ParentPlanetName;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // Delete celestial body
        app.MapDelete("/api/bodies/{id}", [Authorize(Roles = "Manager")] async (string id, AarhusSpaceContext db) =>
        {
            var body = await db.Bodies.FindAsync(id);
            if (body is null)    {
                return Results.NotFound($"Could not find celestial body with name {id}!");
            }

            db.Bodies.Remove(body);
            await db.SaveChangesAsync();

            return Results.Ok($"Celestial body with name {id} has been removed!");
        });

    }
}