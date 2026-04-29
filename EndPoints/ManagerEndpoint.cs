using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using assignment3.Data;
using assignment3.Entities;
using assignment3.DTO;

namespace assignment3.Endpoints;

// ==========================================
// MANAGER CRUD ENDPOINTS
// ==========================================

public static class ManagerEndpoints
{
    public static void MapManagerEndpoints(this IEndpointRouteBuilder app)
    {

        // Create a new manage
        app.MapPost("/api/managers", [Authorize(Roles = "Manager")] async (ManagerCreateDTO createDTO, AarhusSpaceContext db)
        =>
        {
            var newStaff = new Staff
            {
                Name = createDTO.Name,
                HireDate = createDTO.HireDate,
                PayGrade = createDTO.PayGrade
            };

            var newManager = new Manager
            {
                Department = createDTO.Department,
                Staff = newStaff
            };
            db.Managers.Add(newManager);
            await db.SaveChangesAsync();

            var response = new ManagerDTO
            {
                StaffId = newStaff.StaffId,
                Name = newStaff.Name,
                HireDate = newStaff.HireDate,
                PayGrade = newStaff.PayGrade,
                Department = newManager.Department
            };

            return Results.Created($"/api/managers/{response.StaffId}", response);

        });

        // Get all managers
        app.MapGet("/api/managers", [Authorize] async (AarhusSpaceContext db) =>
        {
            var managers = await db.Managers
            .Select(m => new ManagerDTO
            {
                StaffId = m.StaffId,
                Name = m.Staff.Name,
                HireDate = m.Staff.HireDate,
                PayGrade = m.Staff.PayGrade,
                Department = m.Department
            }).ToListAsync();
            
            return Results.Ok(managers);
        });

        // Get a manager by ID
        app.MapGet("/api/managers/{id}", [Authorize] async (int id, AarhusSpaceContext db) =>
        {
            var manager = await db.Managers
            .Where(m => m.StaffId == id)
            .Select(m => new ManagerDTO
            {
                StaffId = m.StaffId,
                Name = m.Staff.Name,
                HireDate = m.Staff.HireDate,
                PayGrade = m.Staff.PayGrade,
                Department = m.Department
            }
            ).FirstOrDefaultAsync();

            return manager is not null ? Results.Ok(manager) : Results.NotFound();
        });

        // Update existing manager
        app.MapPut("/api/managers/{id}", [Authorize(Roles = "Manager")] async (int id, ManagerUpdateDTO updateDTO, AarhusSpaceContext db) =>
        {
            var manager = await db.Managers
            .Include(m => m.Staff)
            .FirstOrDefaultAsync(m => m.StaffId == id);
            
            if (manager is null)
            {
                return Results.NotFound($"Could not find manager with ID {id}!");
            }

            manager.Staff.Name = updateDTO.Name;
            manager.Staff.PayGrade = updateDTO.PayGrade;
            manager.Department = updateDTO.Department;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Delete manager
        app.MapDelete("/api/managers/{id}", [Authorize(Roles = "Manager")] async (int id, AarhusSpaceContext db) =>
        {
            var staff = await db.Staff.FindAsync(id);
            if (staff is null)
            {
                return Results.NotFound($"Could not find manager with ID {id}!");
            }

            db.Staff.Remove(staff);
            await db.SaveChangesAsync();

            return Results.Ok($"Manager with ID {id} has been removed!");

        });
    }
}
