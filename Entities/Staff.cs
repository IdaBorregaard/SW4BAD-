using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;


public class Staff
{
    [Key]
    public int StaffId {get; set;}    

    [StringLength(100)]
    public required string Name {get; set;}

    public required string Role {get; set;}

    public required DateTime HireDate {get; set;}

    public int PayGrade {get; set;}



    // Relationer
    public Manager? ManagerDetails {get; set;}

    public Scientist? ScientistDetails {get; set;}

    public Astronaut? AstonautDetails {get; set;}
}