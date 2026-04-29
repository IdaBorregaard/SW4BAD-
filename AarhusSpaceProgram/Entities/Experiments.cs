namespace assignment3.Entities;

public class Experiment
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int MissionId { get; set; }
    public Mission Mission { get; set; } = null!;

    public int LeadScientistId { get; set; }
    public Scientist LeadScientist { get; set; } = null!;
}