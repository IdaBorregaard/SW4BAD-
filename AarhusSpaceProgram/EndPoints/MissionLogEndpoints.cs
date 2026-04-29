using MongoDB.Driver;
using assignment3.Entities;

namespace assignment3.Endpoints;

public static class MissionLogEndpoints
{
    public static void MapMissionLogEndpoints(this WebApplication app)
    {
        app.MapGet("/api/missions/{id}/logs", async (int id, IConfiguration config) =>
        {
            // Connect to MongoDB using the connection string and database name from configuration
            var mongoUrl = config["MongoDB:ConnectionString"]!;
            var dbName = config["MongoDB:DatabaseName"]!;
            var client = new MongoClient(mongoUrl);
            var db = client.GetDatabase(dbName);
            var logsCollection = db.GetCollection<MissionLogEntry>("MissionLogs");

            // Filter logs by mission ID
            var logs = await logsCollection
                .Find(l => l.MissionId == id)
                .ToListAsync();

            return Results.Ok(logs);
        });
    }
}