using System.ComponentModel.DataAnnotations;

namespace assignment3.DTO;

public class RocketDTO
{
    // From rocket
    public string RocketId {get; set;} = null!;// Primary key
    public string Name {get; set;} = null!; // Name of the rocket
    public float Weight {get; set;} // Weight of the rocket in tons
    public int FuelCap {get; set;} // Fuel capacity of the rocket in tons
    public int Payload {get; set;} // Payload capacity of the rocket in tons
    public int Stages {get; set;} // Number of stages in the rocket (e.g., 2-stage, 3-stage, etc.)
    public int CrewCap {get; set;}// Crew capacity of the rocket (number of astronauts it can carry)
}

public class RocketCreateDTO
{
    // We include ID here because it's a string identifier provided by the user
    public required string RocketId { get; set; } 
    public required string Name { get; set; }
    public float Weight { get; set; }
    public int FuelCap { get; set; }
    public int Payload { get; set; }
    public int Stages { get; set; }
    public int CrewCap { get; set; }
}

public class RocketUpdateDTO
{
    // RocketId is usually not changeable, so we leave it out
    public required string Name { get; set; }
    public float Weight { get; set; }
    public int FuelCap { get; set; }
    public int Payload { get; set; }
    public int Stages { get; set; }
    public int CrewCap { get; set; }
}