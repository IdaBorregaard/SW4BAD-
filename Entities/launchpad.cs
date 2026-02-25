using System.ComponentModel.DataAnnotations;


namespace AarhusSpaceProgram.Models;


public class LaunchPad
{
    [Key] 
    public required string LocationId {get; set;}
    public string Status {get; set;} = "Active";
    public int MaxWeight {get; set;}

    // Relations
    public List<Mission> MissionDetails {get; set;} = null!;

}