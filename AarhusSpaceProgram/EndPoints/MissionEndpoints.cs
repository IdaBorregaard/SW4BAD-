using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
        app.MapPost("/api/missions", [Authorize(Roles = "Manager")] async (MissionCreateDTO createDTO, AarhusSpaceContext db)
        =>
        {
            // Fetch hardware and validate existence
            var targetBody = await db.Bodies.FirstOrDefaultAsync(c => c.Name == createDTO.CelestialDest);
            if (targetBody == null) return Results.BadRequest($"Celestial Body with Name {createDTO.CelestialDest} not found.");

            Rocket? rocket = null;
            if (createDTO.RocketId != null)
            {
                // Validate that the rocket exists, can handle the weight, and is available
                rocket = await db.Rockets.FirstOrDefaultAsync(r => r.RocketId == createDTO.RocketId);
                if (rocket == null) return Results.BadRequest($"Rocket with ID {createDTO.RocketId} not found.");

                var isRocketInUse = await db.Missions.AnyAsync(
                m => m.RocketId == createDTO.RocketId &&
                m.Status != MissionStatus.Completed &&
                m.Status != MissionStatus.Aborted &&
                m.Status != MissionStatus.Failed); // Assuming a rocket can only be used for one active mission at a time
                if (isRocketInUse)
                {
                    return Results.BadRequest($"Rocket with ID {createDTO.RocketId} is already assigned to another mission.");
                }

                // Check capacity of the rocket
                if (createDTO.AstronautIds.Count > rocket.CrewCap)
                {
                    return Results.BadRequest($"Too many astronauts ({createDTO.AstronautIds.Count}) for the selected rocket. Rocket capacity is {rocket.CrewCap}.");
                }
            }

            // Validate that the launch pad exists, can handle the weight, and is available
            var launchPad = await db.LaunchPads.FirstOrDefaultAsync(l => l.LocationId == createDTO.LaunchLocation);
            if (launchPad == null) return Results.BadRequest($"LaunchPad with Location {createDTO.LaunchLocation} not found.");

            var isLaunchPadInUse = await db.Missions.AnyAsync(
                m => m.LaunchLocation == createDTO.LaunchLocation &&
                m.LaunchDate.Date == createDTO.LaunchDate.Date); // Assuming a launch pad can only be used for one mission per day
            if (isLaunchPadInUse)
            {
                return Results.BadRequest($"LaunchPad at Location {createDTO.LaunchLocation} is already assigned to another mission on the same launch date.");
            }

            if (rocket != null)
            {
                var totalWeight = rocket.Weight + rocket.FuelCap + rocket.Payload;
                if (totalWeight > launchPad.MaxWeight)
                {
                    return Results.BadRequest($"The total weight of the rocket ({totalWeight} tons) exceeds the maximum weight capacity of the launch pad ({launchPad.MaxWeight} tons).");
                }
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

            return Results.Created($"/api/missions/{newMission.MissionId}", response);
        });

        // [GET] Get all missions
        app.MapGet("/api/missions", async (AarhusSpaceContext db, string? status) =>
        {
            var query = db.Missions
                .Include(m => m.Crew)
                .Include(m => m.Scientists)
                .Include(m => m.ManagedBy)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<MissionStatus>(status, out var parsedStatus))
            {
                query = query.Where(m => m.Status == parsedStatus);
            }

            var missions = await query.Select(m => new MissionDTO
            {
                MissionId = m.MissionId,
                Name = m.Name,
                LaunchDate = m.LaunchDate,
                LaunchLocation = m.LaunchLocation,
                Status = m.Status,
                RocketId = m.RocketId,
                RocketName = m.Rocket != null ? m.Rocket.Name : "Unassigned",
                CelestialDest = m.CelestialDest,
                ManagerName = m.ManagedBy != null && m.ManagedBy.Staff != null ? m.ManagedBy.Staff.Name : "Unassigned",
                ManagerId = m.ManagerId,
            })
            .ToListAsync();

            return Results.Ok(missions);
        });



        // [GET] Get a mission by ID
        app.MapGet("/api/missions/{id}", [Authorize] async (int id, AarhusSpaceContext db) =>
        {
            var mission = await db.Missions
                .Where(m => m.MissionId == id)
                .Select(m => new MissionDTO
                {
                    MissionId = m.MissionId,
                    Name = m.Name,
                    LaunchDate = m.LaunchDate,
                    LaunchLocation = m.LaunchLocation,
                    Status = m.Status,
                    RocketId = m.RocketId,
                    RocketName = m.Rocket != null ? m.Rocket.Name : "Unassigned",
                    CelestialDest = m.CelestialDest,


                    ManagerName = m.ManagedBy != null && m.ManagedBy.Staff != null ? m.ManagedBy.Staff.Name : "Unassigned",
                    AstronautNames = m.Crew.Select(a => a.Staff != null ? a.Staff.Name : "Unassigned").ToList(),
                    ScientistNames = m.Scientists.Select(s => s.Staff != null ? s.Staff.Name : "Unassigned").ToList(),

                    // And the IDs for the developers:
                    ManagerId = m.ManagerId,
                    AstronautIds = m.Crew.Select(a => a.StaffId).ToList(),
                    ScientistIds = m.Scientists.Select(s => s.StaffId).ToList()

                }).ToListAsync();

            return Results.Ok(mission.FirstOrDefault());
        });



        // [GET] Get a mission by target celestial body
        app.MapGet("/api/missions/destination/{celestialDest}", [Authorize] async (string celestialDest, AarhusSpaceContext db) =>
        {
            var missions = await db.Missions
            .Where(m => m.CelestialDest == celestialDest)
            .Select(m => new MissionDTO
            {
                MissionId = m.MissionId,
                Name = m.Name,
                LaunchDate = m.LaunchDate,
                LaunchLocation = m.LaunchLocation,
                Status = m.Status,
                RocketId = m.RocketId,
                RocketName = m.Rocket != null ? m.Rocket.Name : "Unassigned",
                CelestialDest = m.CelestialDest,

                // få staff navn osv - check op!!
                ManagerName = m.ManagedBy != null && m.ManagedBy.Staff != null ? m.ManagedBy.Staff.Name : "Unassigned",
                AstronautNames = m.Crew.Select(a => a.Staff != null ? a.Staff.Name : "Unassigned").ToList(),
                ScientistNames = m.Scientists.Select(s => s.Staff != null ? s.Staff.Name : "Unassigned").ToList(),

                // And the IDs for the developers:
                ManagerId = m.ManagerId,
                AstronautIds = m.Crew.Select(a => a.StaffId).ToList(),
                ScientistIds = m.Scientists.Select(s => s.StaffId).ToList()

            }).ToListAsync();

            return Results.Ok(missions);
        });


        // [PUT] Update Mission Status
        app.MapPut("/api/missions/{id}", [Authorize(Roles = "Manager")] async (int id, MissionUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var mission = await db.Missions.FindAsync(id);
            if (mission == null) return Results.NotFound($"Mission with ID {id} not found.");

            if (mission.Status == updateDTO.Status) return Results.NoContent();

            bool ValidTransition = mission.Status switch
            {
                MissionStatus.Created => updateDTO.Status is MissionStatus.Budgeted,
                MissionStatus.Budgeted => updateDTO.Status is MissionStatus.Approved,
                MissionStatus.Approved => updateDTO.Status is MissionStatus.Planned,
                MissionStatus.Planned => updateDTO.Status is MissionStatus.Active,
                MissionStatus.Active => updateDTO.Status is MissionStatus.Completed or MissionStatus.Failed or MissionStatus.Aborted,

                MissionStatus.Completed or MissionStatus.Aborted or MissionStatus.Failed => false,

                _ => false
            };

            if (!ValidTransition)
            {
                return Results.BadRequest($"Strict rule violation: Cannot transition mission from '{mission.Status}' directly to '{updateDTO.Status}'.");
            }

            mission.Status = updateDTO.Status;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // [PUT] Assign, Manager, Astronaut or Scientist to a Mission
        app.MapPut("/api/missions/{missionId}/Staff", [Authorize(Roles = "Manager")] async (int missionId, AssignStaffDTO assignDTO, AarhusSpaceContext db) =>
        {
            var mission = await db.Missions
                .Include(m => m.Crew)
                .Include(m => m.Scientists)
                .Include(m => m.Rocket)
                .FirstOrDefaultAsync(m => m.MissionId == missionId);

            if (mission == null) return Results.NotFound($"Mission with ID {missionId} not found.");

            if (assignDTO.ManagerId.HasValue)
            {
                mission.ManagerId = assignDTO.ManagerId;
            }

            // Fetch and assign staff (astronauts and scientists)
            var addCrew = await db.Astronauts.Where(a => assignDTO.AstronautIds.Contains(a.StaffId)).ToListAsync();
            var addScientists = await db.Scientists.Where(s => assignDTO.ScientistIds.Contains(s.StaffId)).ToListAsync();

            foreach (var astronaut in addCrew)
            {
                if (!mission.Crew.Any(c => c.StaffId == astronaut.StaffId))
                {
                    mission.Crew.Add(astronaut);
                }
            }

            foreach (var scientist in addScientists)
            {
                if (!mission.Scientists.Any(c => c.StaffId == scientist.StaffId))
                {
                    mission.Scientists.Add(scientist);
                }
            }

            if (mission.Rocket != null && mission.Crew.Count > mission.Rocket.CrewCap)
            {
                return Results.BadRequest($"Too many astronauts ({mission.Crew.Count}) for the selected rocket. Rocket capacity is {mission.Rocket.CrewCap}.");
            }

            await db.SaveChangesAsync();
            return Results.Ok($"Successfully updated staff for mission {missionId}");
        }
        );


        // [DELETE] Remove staff from a mission (e.g., unassign an astronaut or scientist)
        app.MapDelete("/api/missions/{missionId}/Staff/{staffId}", [Authorize(Roles = "Manager")] async (int missionId, int staffId, AarhusSpaceContext db) =>
        {
            var mission = await db.Missions
                .Include(m => m.Crew)
                .Include(m => m.Scientists)
                .FirstOrDefaultAsync(m => m.MissionId == missionId);

            if (mission == null) return Results.NotFound($"Mission with ID {missionId} not found.");

            bool staffRemoved = false;

            // 1. Check Astronauts
            var astronaut = mission.Crew.FirstOrDefault(a => a.StaffId == staffId);
            if (astronaut != null)
            {
                mission.Crew.Remove(astronaut);
                staffRemoved = true;
            }

            // 2. Check Scientists
            var scientist = mission.Scientists.FirstOrDefault(s => s.StaffId == staffId);
            if (scientist != null)
            {
                mission.Scientists.Remove(scientist);
                staffRemoved = true;
            }

            // 3. Check Manager (Just set it to null!)
            if (mission.ManagerId == staffId)
            {
                mission.ManagerId = null;
                staffRemoved = true;
            }

            if (!staffRemoved)
            {
                return Results.NotFound($"Staff member with ID {staffId} is not assigned to this mission.");
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

    }
}
