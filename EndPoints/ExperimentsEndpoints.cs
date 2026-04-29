using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using assignment3.Data;
using assignment3.DTO;
using assignment3.Entities;

namespace assignment3.Endpoints;

public static class ExperimentEndpoints
{
    public static void MapExperimentEndpoints(this WebApplication app)
    {
        // POST - Scientist and Manager only
        app.MapPost("/api/experiments", [Authorize(Roles = "Scientist,Manager")] async (CreateExperimentDto dto, AarhusSpaceContext db) =>
        {
            var experiment = new Experiment
            {
                Name = dto.Name,
                Description = dto.Description,
                MissionId = dto.MissionId,
                LeadScientistId = dto.LeadScientistId
            };
            db.Experiments.Add(experiment);
            await db.SaveChangesAsync();
            return Results.Created($"/api/experiments/{experiment.Id}", new ExperimentDto(experiment.Id, experiment.Name, experiment.Description, experiment.CreatedAt, experiment.MissionId, experiment.LeadScientistId));
        });

        // GET all experiments - all authenticated users
        app.MapGet("/api/experiments", [Authorize] async (AarhusSpaceContext db) =>
        {
            var experiments = await db.Experiments
                .Select(e => new ExperimentDto(e.Id, e.Name, e.Description, e.CreatedAt, e.MissionId, e.LeadScientistId))
                .ToListAsync();
            return Results.Ok(experiments);
        });

        // GET experiment by ID - all authenticated users
        app.MapGet("/api/experiments/{id}", [Authorize] async (int id, AarhusSpaceContext db) =>
        {
            var experiment = await db.Experiments.FindAsync(id);
            if (experiment == null) return Results.NotFound();
            return Results.Ok(new ExperimentDto(experiment.Id, experiment.Name, experiment.Description, experiment.CreatedAt, experiment.MissionId, experiment.LeadScientistId));
        });

        // PUT - Scientist and Manager only
        app.MapPut("/api/experiments/{id}", [Authorize(Roles = "Scientist,Manager")] async (int id, UpdateExperimentDto dto, AarhusSpaceContext db) =>
        {
            var experiment = await db.Experiments.FindAsync(id);
            if (experiment == null) return Results.NotFound();

            experiment.Name = dto.Name;
            experiment.Description = dto.Description;
            await db.SaveChangesAsync();
            return Results.Ok(new ExperimentDto(experiment.Id, experiment.Name, experiment.Description, experiment.CreatedAt, experiment.MissionId, experiment.LeadScientistId));
        });

        // DELETE - Scientist and Manager only
        app.MapDelete("/api/experiments/{id}", [Authorize(Roles = "Scientist,Manager")] async (int id, AarhusSpaceContext db) =>
        {
            var experiment = await db.Experiments.FindAsync(id);
            if (experiment == null) return Results.NotFound();

            db.Experiments.Remove(experiment);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}