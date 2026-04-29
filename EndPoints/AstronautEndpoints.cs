using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// ASTRONAUT CRUD ENDPOINTS
// ==========================================

public static class AstronautEndpoints
{
    public static void MapAstronautEndpoints(this IEndpointRouteBuilder app)
    {

// Create a new astronaut
app.MapPost("/api/astronauts", [Authorize(Roles = "Manager")] async (AstronautCreateDTO createDTO, AarhusSpaceContext db)
=>{
    var newStaff = new Staff
    {
        Name = createDTO.Name,
        HireDate = createDTO.HireDate,
        PayGrade = createDTO.PayGrade
    };

    var newAstronaut = new Astronaut
    {
        Rank = createDTO.Rank,
        ExperienceSim = createDTO.ExperienceSim,
        ExperienceSpace = createDTO.ExperienceSpace,
        Staff = newStaff
    };
    db.Astronauts.Add(newAstronaut);
    await db.SaveChangesAsync();

     var response = new AstronautDTO
    {
        StaffId = newStaff.StaffId,
        Name = newStaff.Name,
        HireDate = newStaff.HireDate,
        PayGrade = newStaff.PayGrade,
        Rank = newAstronaut.Rank,
        ExperienceSim = newAstronaut.ExperienceSim,
        ExperienceSpace = newAstronaut.ExperienceSpace
    };

    return Results.Created($"/api/astronauts/{response.StaffId}", response);
});

// Get all astronauts
app.MapGet("/api/astronauts", async (AarhusSpaceContext db) =>
{
    var astronauts = await db.Astronauts
    .OrderByDescending(a => a.ExperienceSpace)
    .Select(a => new AstronautDTO
    {
        StaffId = a.StaffId,
        Name = a.Staff.Name,
        HireDate = a.Staff.HireDate,
        PayGrade = a.Staff.PayGrade,
        Rank = a.Rank,
        ExperienceSim = a.ExperienceSim,
        ExperienceSpace = a.ExperienceSpace,
        MissionIds = a.AstronautMission.Select(am => am.MissionId).ToList() // Include list of mission IDs
    }).ToListAsync();

    return Results.Ok(astronauts);
});

// Get an astronaut by ID
app.MapGet("/api/astronauts/{id}", async (int id, AarhusSpaceContext db) =>
{
    var astronaut = await db.Astronauts
    .Where(a => a.StaffId == id)
    .Select(a => new AstronautDTO
    {
        StaffId = a.StaffId,
        Name = a.Staff.Name,
        HireDate = a.Staff.HireDate,
        PayGrade = a.Staff.PayGrade,
        Rank = a.Rank,
        ExperienceSim = a.ExperienceSim,
        ExperienceSpace = a.ExperienceSpace
    }
    ).FirstOrDefaultAsync();

    return astronaut is not null ? Results.Ok(astronaut) : Results.NotFound();
});

// Update existing astronaut
app.MapPut("/api/astronauts/{id}", [Authorize(Roles = "Manager")] async (int id, AstronautUpdateDTO updateDTO, AarhusSpaceContext db) =>
{
    var astronaut = await db.Astronauts
    .Include(a => a.Staff)
    .FirstOrDefaultAsync(a => a.StaffId == id); 

    if (astronaut is null)
    {
        return Results.NotFound($"Could not find astronaut with ID {id}!");
    }

    astronaut.Staff.Name = updateDTO.Name;
    astronaut.Staff.PayGrade = updateDTO.PayGrade;
    astronaut.Rank = updateDTO.Rank;
    astronaut.ExperienceSim = updateDTO.ExperienceSim;
    astronaut.ExperienceSpace = updateDTO.ExperienceSpace;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Delete astronaut
app.MapDelete("/api/astronauts/{id}", [Authorize(Roles = "Manager")] async (int id, AarhusSpaceContext db) =>
{
    var staff = await db.Staff.FindAsync(id);
    if (staff is null)    {
        return Results.NotFound($"Could not find astronaut with ID {id}!");
    }

    db.Staff.Remove(staff);
    await db.SaveChangesAsync();

    return Results.Ok($"Astronaut with ID {id} has been removed!");
});

    }
}