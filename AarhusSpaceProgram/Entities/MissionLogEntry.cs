using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace assignment3.Entities;

public class MissionLogEntry
{
    // MongoDB document ID
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // ID of the mission this log entry belongs to and the log message with a timestamp
    public int MissionId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}