using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;

public class Manager
{
    [Key]
    public int StaffId {get; set;}
    public required string Department {get; set;}



    // Relationer
    public Staff Staff {get; set;} = null!;

    public List<Mission> ManageMission {get; set;} = new(); 
}