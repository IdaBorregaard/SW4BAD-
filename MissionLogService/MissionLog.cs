using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MissionLogService;

public class MissionLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int MissionId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}