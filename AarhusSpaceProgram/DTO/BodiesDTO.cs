using System.ComponentModel.DataAnnotations;
using assignment3.Entities;

namespace assignment3.DTO;

public class CelestialBodyDTO
{
    public string Name {get; set;} = null!;

    public float Dist {get; set;} // Distance from Earth in AU (Astronomical Units)

    public PlanetType BodyType {get; set;} // Type of celestial body (e.g., Rocky, Gas Giant, etc.) - represented by the PlanetType enum

    public string? ParentPlanetName {get; set;}
}

public class CelestialBodyCreateDTO
{
    public required string Name { get; set; }
    public float Dist { get; set; }
    public PlanetType BodyType { get; set; }
    public string? ParentPlanetName { get; set; }
}

public class CelestialBodyUpdateDTO
{
    // Name is omitted to protect the PK
    public float Dist { get; set; }
    public PlanetType BodyType { get; set; }
    public string? ParentPlanetName { get; set; }
}