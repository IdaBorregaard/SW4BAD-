using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

// Enum to represent the type of celestial body
public enum PlanetType
{
    Rocky,
    GasGiant,
    IceGiant,
    Moon,
    DwarfPlanet,
    Asteroid
}

// This class represents celestial bodies such as planets, moons, asteroids, etc.
public class Bodies
{
    [Key] 
    public required string Name {get; set;} // Primary key, unique name for each celestial body

    public float Dist {get; set;} // Distance from Earth in AU (Astronomical Units)

    public PlanetType BodyType {get; set;} // Type of celestial body (e.g., Rocky, Gas Giant, etc.) - represented by the PlanetType enum


    // Relations
    public List<Mission> TargetBody {get; set;} = null!; // Missions that have this body as their target
    public string? ParentPlanetName {get; set;} // Foreign key to reference the parent planet (if this body is a moon)
    public Bodies? ParentPlanet {get; set;} // Navigation property to the parent planet (if this body is a moon)
    public List<Bodies> Moons {get; set;} = new(); // List of moons orbiting this body (if this body is a planet)
}