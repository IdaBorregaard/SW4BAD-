using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

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

    public string? ParentPlanetName {get; set;}

    public Bodies? ParentPlanet {get; set;}

    public List<Bodies> Moons {get; set;} = new();
}