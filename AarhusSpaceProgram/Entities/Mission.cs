using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment3.Entities;

// Enumeration for the type of mission, which can be either Orbit, Landing, or Flyby.
public enum MissionType
{
    Orbit,
    Landing,
    Flyby
}

// Enumeration for the status of a mission, which can be Planned, Ongoing, Completed, or Aborted.
public enum MissionStatus
{
    Created,
    Budgeted,
    Approved,
    Planned,
    Active,
    Completed,
    Aborted,
    Failed
}

// This class represents a space mission and its constraints and relations to other entities.
public class Mission
{
    [Key]
    public int MissionId { get; set; } // Primary key, unique identifier for each mission
    public required string Name { get; set; } // Name of the mission
    public DateTime LaunchDate { get; set; } // Date of the mission launch 
    public float Duration { get; set; } // Duration of the mission in days
    public required MissionStatus Status { get; set; } // Status of the mission: use the MissionStatus enumeration defined above
    public MissionType Type { get; set; } // Type of the mission - the enumeration defined above

    // Relations to other entities via foreign keys

    // 1:1 with Rocket
    public string? RocketId { get; set; } // Foreign key to reference the rocket used for the mission
    [ForeignKey("RocketId")]
    public Rocket? Rocket { get; set; }

    // 1:N with LaunchPad: explicitly tell EF that 'LaunchLocation' points to the LaunchPad
    public required string LaunchLocation { get; set; } // Foreign key to reference the launch pad used for the mission
    [ForeignKey("LaunchLocation")]
    public LaunchPad LaunchPad { get; set; } = null!;

    // 1:N with CelestialBody
    public required string CelestialDest { get; set; } // Foreign key to reference the celestial body that is the target of the mission
    [ForeignKey("CelestialDest")]
    public CelestialBody CelestialBody { get; set; } = null!;

    // 1:N with Manager: a mission can have one manager, but a manager can be responsible for many missions
    public int? ManagerId { get; set; }
    [ForeignKey("ManagerId")]
    public Manager? ManagedBy { get; set; }

    // N:N Relationships: list of astronauts and scientists involved in the mission
    public List<Astronaut> Crew { get; set; } = new();
    public List<Scientist> Scientists { get; set; } = new();

}