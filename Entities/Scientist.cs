using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

// This class represents a scientist involved in space missions.
public class Scientist
{
    [Key]
    public int StaffId {get; set;} // Primary key
    public string Title {get; set;} = null!; // Title of the scientist (e.g., Dr., Prof., etc.)
    public string Speciality {get; set;} = null!; // Area of expertise or speciality of the scientist
    
    // Relations to Staff and Mission
    public Staff Staff {get; set;} = null!; // 1:1 relationship, as each scientist is also a staff member
    public List<Mission> ScientistMission {get; set;} = new(); // 1:N relationship, as one scientist can be involved in multiple missions

}