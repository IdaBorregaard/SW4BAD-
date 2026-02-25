using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;


public class Scientist
{
    [Key]
    public int StaffId {get; set;}

    public string Title {get; set;} = null!;

    public string Speciality {get; set;} = null!;
    
    
    // Relationer
    public Staff Staff {get; set;} = null!;

    public List<Mission> AstronautMission {get; set;} = new();

}