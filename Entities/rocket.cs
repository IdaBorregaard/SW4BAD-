using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

/*
This class represents a rocket used in space missions.
Relations is 1:1 with Mission, as each mission can only use one rocket, and each rocket can only be used for one mission.
*/
public class Rocket
{
    [Key] 
    public required string RocketId {get; set;} // Primary key
    public required string Name {get; set;} // Name of the rocket
    public float Weight {get; set;} // Weight of the rocket in tons
    public int FuelCap {get; set;} // Fuel capacity of the rocket in tons
    public int Payload {get; set;} // Payload capacity of the rocket in tons
    public int Stages {get; set;} // Number of stages in the rocket (e.g., 2-stage, 3-stage, etc.)
    public int CrewCap {get; set;} // Crew capacity of the rocket (number of astronauts it can carry)

    // Relation to Mission - 1:1 relationship, as each mission can only use one rocket, and each rocket can only be used for one mission
    public Mission RocketUsed {get; set;} = null!;
}