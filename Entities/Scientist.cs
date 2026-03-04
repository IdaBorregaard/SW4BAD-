using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;


public class Scientist
{
    [Key]
    public int StaffId {get; set;}

    public string Title {get; set;} = null!;

    public string Speciality {get; set;} = null!;
    
    
    // Relationer
    public Staff Staff {get; set;} = null!;

    public List<Mission> ScientistMission {get; set;} = new();

}