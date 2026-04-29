using System.ComponentModel.DataAnnotations;

namespace assignment3.DTO;

public class AstronautDTO
{
    // From staff
    public int StaffId {get; set;}
    public string Name {get; set;} = null!;
    public DateTime HireDate {get; set;}
    public int PayGrade {get; set;}

    // From astronaut
    public string Rank {get; set;} = null!;
    public float ExperienceSim {get; set;}
    public float ExperienceSpace {get; set;}

    // From missions
    public List<int> MissionIds {get; set;} = new();
}

// For POST request, as StaffId is auto-generated and not provided by the client
public class AstronautCreateDTO
{
    public required string Name { get; set; }
    public DateTime HireDate { get; set; }
    public int PayGrade { get; set; }
    public required string Rank { get; set; }
    public float ExperienceSim { get; set; }
    public float ExperienceSpace { get; set; }
}

// For PUT request, as StaffId is provided in the URL and not in the body
public class AstronautUpdateDTO
{
    public required string Name { get; set; }
    public int PayGrade { get; set; }
    public required string Rank { get; set; }
    public float ExperienceSim { get; set; }
    public float ExperienceSpace { get; set; }
}