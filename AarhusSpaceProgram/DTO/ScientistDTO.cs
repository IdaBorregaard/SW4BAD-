using System.ComponentModel.DataAnnotations;

namespace assignment3.DTO;

public class ScientistDTO
{
    // From staff
    public int StaffId {get; set;} 
    public string Name {get; set;} = null!;
    public DateTime HireDate {get; set;}
    public int PayGrade {get; set;}

    // From Scientist
    public string Title {get; set;} = null!;
    public string Speciality {get; set;} = null!;

    public List<int> MissionIds {get; set;} = new();
}

// For POST request, as StaffId is auto-generated and not provided by the client
public class ScientistCreateDTO
{
    public required string Name { get; set; }
    public DateTime HireDate { get; set; }
    public int PayGrade { get; set; }
    public required string Title { get; set; }
    public required string Speciality { get; set; }
}

// For PUT request, as StaffId is provided in the URL and not in the body
public class ScientistUpdateDTO
{
    public required string Name { get; set; }
    public int PayGrade { get; set; }
    public required string Title { get; set; }
    public required string Speciality { get; set; }
}