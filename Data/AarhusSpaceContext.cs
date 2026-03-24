using Microsoft.EntityFrameworkCore;
using assignment3.Entities;

namespace assignment3.Data;

/* 
This class represents the database context for the Aarhus Space program. 
It defines the DbSets for each entity and configures the relationships and seed data.
*/
public class AarhusSpaceContext : DbContext
{
    // Constructor to pass options to the base DbContext
    public AarhusSpaceContext(DbContextOptions<AarhusSpaceContext> options)
        : base(options)
    {
    }

    // DbSets representing the tables in the database
    public DbSet<Staff> Staff { get; set; } = null!;
    public DbSet<Manager> Managers { get; set; } = null!;
    public DbSet<Astronaut> Astronauts { get; set; } = null!;
    public DbSet<LaunchPad> LaunchPads { get; set; } = null!;
    public DbSet<CelestialBody> Bodies { get; set; } = null!;
    public DbSet<Mission> Missions { get; set; } = null!;
    public DbSet<Rocket> Rockets { get; set; } = null!;
    public DbSet<Scientist> Scientists { get; set; } = null!;


    // Relations and seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Staff - 1:1 relationer til Manager, Astronaut og Scientist
        modelBuilder.Entity<Staff>()
             .HasOne(s => s.ManagerDetails)
             .WithOne(m => m.Staff)
             .HasForeignKey<Manager>(m => m.StaffId);

        modelBuilder.Entity<Staff>()
            .HasOne(s => s.AstonautDetails)
            .WithOne(a => a.Staff)
            .HasForeignKey<Astronaut>(a => a.StaffId);

        modelBuilder.Entity<Staff>()
            .HasOne(s => s.ScientistDetails)
            .WithOne(sc => sc.Staff)
            .HasForeignKey<Scientist>(sc => sc.StaffId);



        // Missions
        // 1:N - Mission has one Manager, but a Manager can manage many Missions
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.ManagedBy) 
            .WithMany(ma => ma.ManageMission); 

        // N:N - Mission has many Astronauts, and an Astronaut can be part of many Missions
        modelBuilder.Entity<Mission>()
            .HasMany(m => m.Crew)
            .WithMany(c => c.AstronautMission);

        // N:N - Mission has many Scientists, and a Scientist can be part of many Missions
        modelBuilder.Entity<Mission>()
            .HasMany(m => m.Scientists) 
            .WithMany(s => s.ScientistMission); // Hidden join table "MissionScientist"

        // 1:N - Each Mission has one LaunchPad, but a LaunchPad can be used for many Missions
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.LaunchPad)
            .WithMany(l => l.MissionDetails); // Hidden join table "LaunchPadMission"

        // 1:1 - Each Mission has one Rocket, and each Rocket can only be used for one Mission
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Rocket)
            .WithOne(r => r.RocketUsed)
            .HasForeignKey<Mission>(m => m.RocketId);

        // 1:N - Mission has one CelestialBody, but a CelestialBody can be the target of many Missions
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.CelestialBody) 
            .WithMany(b => b.TargetMissions);

        modelBuilder.Entity<CelestialBody>()
            .HasOne(b => b.ParentPlanet)        // Moon has one parent (planet)
            .WithMany(p => p.Moons)             // Planet has many moons
            .HasForeignKey(b => b.ParentPlanetName) // String name as FK
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid deleting planets when moons are deleted


        // Seed data
        // Rockets - 2
        modelBuilder.Entity<Rocket>().HasData(
            new Rocket { RocketId = "SN-01", Name = "Falcon-X", Weight = 5000000, FuelCap = 4800000, Payload = 140000, Stages = 2, CrewCap = 5 },
            new Rocket { RocketId = "SN-02", Name = "Saturn-V", Weight = 2800000, FuelCap = 2100000, Payload = 130000, Stages = 3, CrewCap = 3 }
        );

        // Launchpads - 2
        modelBuilder.Entity<LaunchPad>().HasData(
            new LaunchPad { LocationId = "Pad-Alpha", Status = "Active", MaxWeight = 1000000 },
            new LaunchPad { LocationId = "Pad-Beta", Status = "Active", MaxWeight = 800000 }
        );

        // Staff - 11 (These must match the StaffId of the Managers, Astronauts and Scientists tables to satisfy the FK constraint)
        modelBuilder.Entity<Staff>().HasData(
            new Staff { StaffId = 1, Name = "Rachel Adams", HireDate = new DateTime(2016, 08, 01), PayGrade = 2 },
            new Staff { StaffId = 2, Name = "Jane Smith", HireDate = new DateTime(2010, 06, 01), PayGrade = 4 },
            new Staff { StaffId = 101, Name = "Jessica Day", HireDate = new DateTime(2020, 02, 01), PayGrade = 5 },
            new Staff { StaffId = 102, Name = "Nick Miller", HireDate = new DateTime(2018, 12, 01), PayGrade = 6},
            new Staff { StaffId = 103, Name = "Ali Hazelwood", HireDate = new DateTime(2012, 08, 01), PayGrade = 7},
            new Staff { StaffId = 201, Name = "Rick Roll", HireDate = new DateTime(2016, 04, 01), PayGrade = 8},
            new Staff { StaffId = 202, Name = "Amanda Smith", HireDate = new DateTime(2011, 09, 01), PayGrade = 9},
            new Staff { StaffId = 203, Name = "Damon Salvatore", HireDate = new DateTime(2009, 11, 01), PayGrade = 9},
            new Staff { StaffId = 204, Name = "Bella Hadid", HireDate = new DateTime(2023, 04, 01), PayGrade = 7},
            new Staff { StaffId = 205, Name = "Bridget Mendler", HireDate = new DateTime(2020, 12, 01), PayGrade = 8}
        );

        // Managers 2 - (These must match the StaffId of the Staff table to satisfy the FK constraint)
        modelBuilder.Entity<Manager>().HasData(
            new Manager { StaffId = 1, Department = "Ground Support" },
            new Manager { StaffId = 2, Department = "Flight Ops" }
        );

        // Scientists 3 (These must match the StaffId of the Staff table to satisfy the FK constraint)
        modelBuilder.Entity<Scientist>().HasData(
            new Scientist { StaffId = 101, Title = "Phd", Speciality = "Biology" },
            new Scientist { StaffId = 102, Title = "Dr", Speciality = "Physics" },
            new Scientist { StaffId =103, Title = "Proffesor", Speciality = "Astronomy"}
        );

        // Astronauts 5 - (These must match the StaffId of the Staff table to satisfy the FK constraint)
        modelBuilder.Entity<Astronaut>().HasData(
            new Astronaut { StaffId = 201, Rank = "Pilot", ExperienceSim = 4800, ExperienceSpace = 4300},
            new Astronaut { StaffId = 202, Rank = "Commander", ExperienceSim = 7200, ExperienceSpace = 8600 },
            new Astronaut { StaffId = 203, Rank = "Commander", ExperienceSim = 8500, ExperienceSpace = 10500 },
            new Astronaut { StaffId = 204, Rank = "Astronaut Candidate", ExperienceSim = 1500, ExperienceSpace = 0 },
            new Astronaut { StaffId = 205, Rank = "Mission Specialt", ExperienceSim = 3200, ExperienceSpace = 4100 }
        );


        // Celestial Bodies - Recursive relation (Parent-Child) for Planets and Moons
        modelBuilder.Entity<CelestialBody>().HasData(
            // Planets - Parent
            new CelestialBody { Name = "Earth", Dist = 1.0f, BodyType = PlanetType.Rocky},
            new CelestialBody { Name = "Jupiter", Dist = 5.20f, BodyType = PlanetType.GasGiant},
            

            // Moons - Child
            new CelestialBody { Name = "Lunar", Dist = 1.002f, BodyType = PlanetType.Moon, ParentPlanetName = "Earth"},
            new CelestialBody { Name = "Europa", Dist = 5.201f, BodyType = PlanetType.Moon, ParentPlanetName = "Jupiter"}
        );


        // Missions
        modelBuilder.Entity<Mission>().HasData(
            new Mission {
                MissionId = 1,
                Name = "Mission X", 
                LaunchDate = new DateTime(2034, 03, 16, 9, 0, 0), 
                Duration = 27.0f, 
                Status = MissionStatus.Planned, 
                Type = MissionType.Landing, 
                // Foreign Keys (These must match the [Key] of the connected tables!)
                RocketId = "SN-01", 
                LaunchLocation = "Pad-Beta", 
                CelestialDest = "Lunar",
                ManagerId = 2
                },
            
            new Mission {
                MissionId = 2, 
                Name = "Apollo 11", 
                LaunchDate = new DateTime(2045, 07, 1, 10, 0, 0), 
                Duration = 14.5f, 
                Status = MissionStatus.Planned, 
                Type = MissionType.Flyby, 
                // Foreign Keys (These must match the [Key] of the connected tables!)
                RocketId = "SN-02", 
                LaunchLocation = "Pad-Alpha", 
                CelestialDest = "Jupiter",
                ManagerId = 1
                }
        );

        // Hidden join tables for N:N relationer
        // Assign the Crew (astronauts) to the Missions
        modelBuilder.Entity("AstronautMission").HasData(
            new { AstronautStaffId = 201, MissionName = "Mission X" },
            new { AstronautStaffId = 202, MissionName = "Mission X" },
            new { AstronautStaffId = 203, MissionName = "Mission X" },
            new { AstronautStaffId = 204, MissionName = "Mission X" },
            new { AstronautStaffId = 205, MissionName = "Mission X" },
            new { AstronautStaffId = 202, MissionName = "Apollo 11" },
            new { AstronautStaffId = 203, MissionName = "Apollo 11" }
        );

        // Assign the Scientists to the Missions
        modelBuilder.Entity("ScientistMission").HasData(
            new { ScientistStaffId = 101, MissionName = "Mission X" },
            new { ScientistStaffId = 102, MissionName = "Mission X" },
            new { ScientistStaffId = 101, MissionName = "Apollo 11" },
            new { ScientistStaffId = 102, MissionName = "Apollo 11" },
            new { ScientistStaffId = 103, MissionName = "Apollo 11" }
        ); 

    }

}