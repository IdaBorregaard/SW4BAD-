using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// MISSION CRUD ENDPOINTS
// ==========================================

public static class MissionEndpoints
{
    public static void MapMissionsEndpoints(this IEndpointRouteBuilder app)
    {
        // Create a new mission
        app.MapPost("/api/missions", async (MissionCreateDTO createDTO, AarhusSpaceContext db)
        =>{
            // Fetch hardware and validate existence
            var rocket = await db.Rockets.FirstOrDefaultAsync(r => r.RocketId == createDTO.RocketId);
            var launchPad = await db.LaunchPads.FirstOrDefaultAsync(l => l.LocationId == createDTO.LaunchLocation);
            var targetBody = await db.Bodies.FirstOrDefaultAsync(c => c.Name == createDTO.CelestialDest);
            
            if (rocket == null) return Results.BadRequest($"Rocket with ID {createDTO.RocketId} not found.");
            if (launchPad == null) return Results.BadRequest($"LaunchPad with Location {createDTO.LaunchLocation} not found.");
            if (targetBody == null) return Results.BadRequest($"Celestial Body with Name {createDTO.CelestialDest} not found.");

            // Check capacity of the rocket
            if (createDTO.AstronautIds.Count > rocket.CrewCap)
            {
                return Results.BadRequest($"Too many astronauts ({createDTO.AstronautIds.Count}) for the selected rocket. Rocket capacity is {rocket.CrewCap}.");
            }

            // Fetch and assign staff (astronauts and scientists)
            var crew = await db.Astronauts.Where(a => createDTO.AstronautIds.Contains(a.StaffId)).ToListAsync();
            var scientists = await db.Scientists.Where(s => createDTO.ScientistIds.Contains(s.StaffId)).ToListAsync();

            // Create the new mission entity
            var newMission = new Mission
            {
                Name = createDTO.Name,
                LaunchDate = createDTO.LaunchDate,
                Duration = createDTO.Duration,
                Status = createDTO.Status,
                Type = createDTO.Type,
                RocketId = createDTO.RocketId,
                LaunchLocation = createDTO.LaunchLocation,
                CelestialDest = createDTO.CelestialDest,
                ManagerId = createDTO.ManagerId,

                // Link the crew and scientists to the mission
                Crew = crew,
                Scientists = scientists
            };

            db.Missions.Add(newMission);
            await db.SaveChangesAsync();

            var response = new MissionDTO
            {
                MissionId = newMission.MissionId,
                Name = newMission.Name,
                LaunchDate = newMission.LaunchDate,
                Duration = newMission.Duration,
                Status = newMission.Status,
                Type = newMission.Type,
                RocketId = newMission.RocketId,
                LaunchLocation = newMission.LaunchLocation,
                CelestialDest = newMission.CelestialDest,

                ManagerId = newMission.ManagerId,
                AstronautIds = crew.Select(a => a.StaffId).ToList(),
                ScientistIds = scientists.Select(s => s.StaffId).ToList()
            };

            return Results.Created($"/api/missions/{newMission.Name}", newMission);
        });

        // [GET] Get all missions
        app.MapGet("/api/missions", async (AarhusSpaceContext db) =>
{
    var missions = await db.Missions
        .Include(m => m.Crew)        // Tell EF to join the Astronauts table
        .Include(m => m.Scientists)  // Tell EF to join the Scientists table
        .Include(m => m.ManagedBy)   // Tell EF to join the Managers table
        .Select(m => new MissionDTO
        {
            MissionId = m.MissionId,
            Name = m.Name,
            LaunchDate = m.LaunchDate,
            Status = m.Status,
            RocketId = m.RocketId,

            // få staff navn osv - check op!!
            
            // And the IDs for the developers:
            AstronautIds = m.Crew.Select(a => a.StaffId).ToList(),
            ScientistIds = m.Scientists.Select(s => s.StaffId).ToList()
        })
        .ToListAsync();

    return Results.Ok(missions);
});



        // [PUT] Update Mission Status
        app.MapPut("/api/missions/{id}", async (int id, MissionUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var mission = await db.Missions.FindAsync(id);
            if (mission == null) return Results.NotFound($"Mission with ID {id} not found.");

            // Validate requirement C: Only allow status updates to valid transitions (e.g., Planned -> Ongoing -> Completed/Aborted)
            mission.Status = updateDTO.Status;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

    }
}
