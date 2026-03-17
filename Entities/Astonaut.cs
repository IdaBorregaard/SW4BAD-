using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

// This class represents an astronaut involved in space missions.
public class Astronaut
{
    [Key]
    public int StaffId {get; set;} // Primary key
    required public string Rank {get; set;} // Rank of the astronaut (e.g., Commander, Pilot, Mission Specialist, etc.)
    public float ExperienceSim {get; set;} // Experience in space simulations in hours
    public float ExperienceSpace {get; set;} // Experience in actual space missions in hours

    // Relations to Staff and Mission
    public Staff Staff {get; set;} = null!; // 1:1 relationship, as each astronaut is also a staff member
    public List<Mission> AstronautMission {get; set;} = new(); // 1:N relationship, as one astronaut can be involved in multiple missions

}