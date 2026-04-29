using System.ComponentModel.DataAnnotations;


namespace assignment3.Entities;

/*
This class represents a launch pad used for space missions.
Relations is 1:N with Mission, as one launch pad can be used for multiple missions, but each mission can only use one launch pad.
*/
public class LaunchPad
{
    [Key] 
    public required string LocationId {get; set;} // Primary key
    public string Status {get; set;} = "Active"; // Status of the launch pad (Default is "Active", but can be set to "Inactive" if the launch pad is not operational)
    public int MaxWeight {get; set;} // Maximum weight capacity of the launch pad in tons

    // Relation: 1:N (One LaunchPad -> Many Missions)
    // IMPORTANT: Initialize with new() to avoid NullReferenceExceptions
    // when you try to add a mission to this list later!
    public List<Mission> MissionDetails {get; set;} = new();
}