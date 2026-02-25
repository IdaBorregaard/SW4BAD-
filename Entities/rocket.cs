using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;


public class Rocket
{
    [Key] 
    public required string RocketId {get; set;}

    public required string Name {get; set;}
    public float Weight {get; set;} 

    public int FuelCap {get; set;}

    public int Payload {get; set;}

    public int Stages {get; set;}

    public int CrewCap {get; set;}


    // Relation
    public required Mission RocketUsed {get; set;}


}