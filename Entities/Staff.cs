using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

/*
This class represents a staff member involved in space missions, which can be a manager, scientist, or astronaut. 
Each staff member has a unique StaffId and can have specific details based on their role. 
*/
public class Staff
{
    [Key]
    public int StaffId {get; set;} // Primary key, unique identifier for each staff member

    [StringLength(100)] 
    public required string Name {get; set;} // Name of the staff member, required and with a maximum length of 100 characters
   // public required string Role {get; set;}
    public required DateTime HireDate {get; set;} // Date when the staff member was hired, required
    public int PayGrade {get; set;} // Pay grade of the staff member

    // Relations to specific roles - 1:1 relationships, as each staff member can only have one role (manager, scientist, or astronaut)
    public Manager? ManagerDetails {get; set;}
    public Scientist? ScientistDetails {get; set;}
    public Astronaut? AstonautDetails {get; set;}
}