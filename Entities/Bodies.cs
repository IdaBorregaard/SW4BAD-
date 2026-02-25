using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;

public enum PlanetType
{
    Rocky,
    GasGiant
}



public class Bodies
{
    [Key] 
    public required string Name {get; set;}

    public float Dist {get; set;}

    public required string BodyType {get; set;}


    // Relationer

    public List<Mission> TargetBody {get; set;} = null!;
}



public class Planet : Bodies
{
    public PlanetType Type {get; set;}
}

public class Moon : Bodies
{
    public Planet? ParentPlanet {get; set;}
}