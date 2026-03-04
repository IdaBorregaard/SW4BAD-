using System.ComponentModel.DataAnnotations;

namespace assignment3.Entities;

public enum MissionType
{
    Orbit,
    Landing,
    Flyby
}

public class Mission
{
    [Key]
    public required string Name {get; set;}

    public DateTime LaunchDate {get; set;}

    public float Duration {get; set;}

    public required string Status {get; set;}

    public MissionType Type {get; set;}

    public required string RocketId {get; set;}

    public required string LaunchLocation {get; set;}

    public required string CelestialDest {get; set;}

    // Staff
    public int ManagerId {get; set;}

    public int CrewId {get; set;}

    public int ScientistId {get; set;}

    // Relation

    public Manager ManagedBy {get; set;} = null!;

    public List<Astronaut> Crew {get; set;} = null!;

    public List<Scientist> Scientists {get; set;} = null!;

    public Rocket Rocket {get; set;} = null!;

    public LaunchPad LaunchPad {get; set;} = null!;

    public Bodies CelestialBody {get; set;} = null!;

}