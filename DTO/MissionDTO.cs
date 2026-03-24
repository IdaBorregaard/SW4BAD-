using System.ComponentModel.DataAnnotations;
using assignment3.Entities;

namespace assignment3.DTO;

public class MissionDTO
{
    // From Mission
    public int MissionId {get; set;} // Primary key, unique identifier for each mission
    public string Name {get; set;} = null!;
    public DateTime LaunchDate {get; set;} // Date of the mission launch 
    public float Duration {get; set;} // Duration of the mission in days
    public MissionStatus Status {get; set;} // Status of the mission
    public MissionType Type {get; set;} // Type of the mission - the enumeration defined above
    
    // From other entites
    public string RocketId {get; set;} = null!; // Foreign key to reference the rocket used for the mission
    public string LaunchLocation {get; set;} = null!; // Foreign key to reference the launch pad used for the mission
    public string CelestialDest {get; set;} = null!; // Foreign key to reference the celestial body that is the target of the mission
    
    public int? ManagerId {get; set;} // Foreign key to reference the manager responsible for the mission
    public string? ManagerName {get; set;}
    
    public List<int> AstronautIds {get; set;} = new();
    public List<string> AstronautNames {get; set;} = new();

    public List<int> ScientistIds {get; set;} = new();
    public List<string> ScientistNames {get; set;} = new();
}

public class MissionCreateDTO
{
    public required string Name { get; set; }
    public DateTime LaunchDate { get; set; }
    public float Duration { get; set; }
    public MissionType Type { get; set; }
    public MissionStatus Status { get; set; }
    
    // Hardware/Location links
    public required string RocketId { get; set; }
    public required string LaunchLocation { get; set; }
    public required string CelestialDest { get; set; }

    // Personnel links (IDs used to "wire up" the mission)
    public int? ManagerId { get; set; }
    public List<int> AstronautIds { get; set; } = new();
    public List<int> ScientistIds { get; set; } = new();
}

public class MissionUpdateDTO
{
    public MissionStatus Status { get; set; } // Requirement C: Update the status of a mission
    public DateTime LaunchDate { get; set; }
    public float Duration { get; set; }
    public int? ManagerId { get; set; }
}