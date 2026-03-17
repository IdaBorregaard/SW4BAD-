using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

// This class represents a manager involved in space missions.
public class Manager
{
    [Key]
    public int StaffId {get; set;} // Primary key
    public required string Department {get; set;} // Department the manager is responsible for

    // Relations to Staff and Mission
    public Staff Staff {get; set;} = null!; // 1:1 relationship, as each manager is also a staff member
    public List<Mission> ManageMission {get; set;} = new(); // 1:N relationship, as one manager can be responsible for multiple missions
}