using System.ComponentModel.DataAnnotations;

namespace assignment3.DTO;

public class ManagerCreateDTO
{
    public required string Name { get; set; }
    public DateTime HireDate { get; set; }
    public int PayGrade { get; set; }
    public required string Department { get; set; }
}

public class ManagerDTO
{
    public int StaffId {get; set;} 
    public string Name {get; set;} = null!;
    public DateTime HireDate {get; set;}
    public int PayGrade {get; set;}
    public string? Department {get; set;}
}

public class ManagerUpdateDTO
{
    // We leave out StaffId because it's in the URL
    // We leave out HireDate because it's usually "set in stone"
    public required string Name { get; set; }
    public int PayGrade { get; set; }
    public required string Department { get; set; }
}
