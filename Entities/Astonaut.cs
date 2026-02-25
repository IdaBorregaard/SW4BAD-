using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;

public class Astronaut
{
    [Key]
    public int StaffId {get; set;}

    public int Rank {get; set;}

    public float ExperienceSim {get; set;}

    public float ExperienceSpace {get; set;}


    // Relationer
    public Staff Staff {get; set;} = null!;

    public List<Mission> AstronautMission {get; set;} = new(); 
    
}