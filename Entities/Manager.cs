using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

public class Manager
{
    [Key]
    public int StaffId {get; set;}
    public required string Department {get; set;}



    // Relationer
    public Staff Staff {get; set;} = null!;

    public List<Mission> ManageMission {get; set;} = new(); 
}