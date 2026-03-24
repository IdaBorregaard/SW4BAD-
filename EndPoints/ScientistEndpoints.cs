using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// SCIENTIST CRUD ENDPOINTS
// ==========================================

public static class ScientistEndpoints
{
    public static void MapScientistEndpoints(this IEndpointRouteBuilder app)
    {

        // Create a new scientist
        app.MapPost("/api/scientists", async (ScientistCreateDTO createDTO, AarhusSpaceContext db)
        =>
        {
            var newStaff = new Staff
            {
                Name = createDTO.Name,
                HireDate = createDTO.HireDate,
                PayGrade = createDTO.PayGrade
            };

            var newScientist = new Scientist
            {
                Title = createDTO.Title,
                Speciality = createDTO.Speciality,
                Staff = newStaff
            };
            db.Scientists.Add(newScientist);
            await db.SaveChangesAsync();

            var response = new ScientistDTO
            {
                StaffId = newStaff.StaffId,
                Name = newStaff.Name,
                HireDate = newStaff.HireDate,
                PayGrade = newStaff.PayGrade,
                Title = newScientist.Title,
                Speciality = newScientist.Speciality
            };

            return Results.Created($"/api/scientists/{response.StaffId}", response);
        });

        // Get all scientists
        app.MapGet("/api/scientists", async (AarhusSpaceContext db) =>
        {
            var scientists = await db.Scientists
            .Include(s => s.Staff)
            .Select(s => new ScientistDTO
            {
                StaffId = s.StaffId,
                Name = s.Staff.Name,
                HireDate = s.Staff.HireDate,
                PayGrade = s.Staff.PayGrade,
                Title = s.Title,
                Speciality = s.Speciality
            }).ToListAsync();

            return Results.Ok(scientists);
        });

        // Get a scientist by ID
        app.MapGet("/api/scientists/{id}", async (int id, AarhusSpaceContext db) =>
        {
            var scientist = await db.Scientists
            .Include(s => s.Staff)
            .Where(s => s.StaffId == id)
            .Select(s => new ScientistDTO
            {
                StaffId = s.StaffId,
                Name = s.Staff.Name,
                HireDate = s.Staff.HireDate,
                PayGrade = s.Staff.PayGrade,
                Title = s.Title,
                Speciality = s.Speciality
            }
            ).FirstOrDefaultAsync();

            return scientist is not null ? Results.Ok(scientist) : Results.NotFound();
        });

        // Update existing scientist
        app.MapPut("/api/scientists/{id}", async (int id, ScientistUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var scientist = await db.Scientists
            .Include(s => s.Staff)
            .FirstOrDefaultAsync(s => s.StaffId == id);

            if (scientist is null)
            {
                return Results.NotFound($"Could not find scientist with ID {id}!");
            }

            scientist.Staff.Name = updateDTO.Name;
            scientist.Staff.PayGrade = updateDTO.PayGrade;
            scientist.Title = updateDTO.Title;
            scientist.Speciality = updateDTO.Speciality;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Delete scientist
        app.MapDelete("/api/scientists/{id}", async (int id, AarhusSpaceContext db) =>
        {
            var staff = await db.Staff.FindAsync(id);
            if (staff is null)
            {
                return Results.NotFound($"Could not find scientist with ID {id}!");
            }

            db.Staff.Remove(staff);
            await db.SaveChangesAsync();
            return Results.Ok($"Scientist with ID {id} has been removed!");
        });
    }
}