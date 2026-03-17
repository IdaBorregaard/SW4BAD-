using System.ComponentModel.DataAnnotations;

namespace assignment3.Entities;

// Enumeration for the type of mission, which can be either Orbit, Landing, or Flyby.
public enum MissionType
{
    Orbit,
    Landing,
    Flyby
}

// This class represents a space mission and its constraints and relations to other entities.
public class Mission
{
    [Key]
    public required string Name {get; set;} // Primary key, name of the mission
    public DateTime LaunchDate {get; set;} // Date of the mission launch 
    public float Duration {get; set;} // Duration of the mission in days
    public required string Status {get; set;} // Status of the mission (e.g., Planned, Ongoing, Completed, etc.)
    public MissionType Type {get; set;} // Type of the mission - the enumeration defined above
    public required string RocketId {get; set;} // Foreign key to reference the rocket used for the mission
    public required string LaunchLocation {get; set;} // Foreign key to reference the launch pad used for the mission
    public required string CelestialDest {get; set;} // Foreign key to reference the celestial body that is the target of the mission
    public int ManagerId {get; set;} // Foreign key to reference the manager responsible for the mission

    // Relations to other entities
    public Manager ManagedBy {get; set;} = null!; // 1:N relation

    public List<Astronaut> Crew {get; set;} = null!; // 1:N relation

    public List<Scientist> Scientists {get; set;} = null!; // 1:N relation

    public Rocket Rocket {get; set;} = null!; // 1:1 relation

    public LaunchPad LaunchPad {get; set;} = null!; // 1:N relation

    public Bodies CelestialBody {get; set;} = null!; // 1:N relation
}