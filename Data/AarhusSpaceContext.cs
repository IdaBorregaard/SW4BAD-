using Microsoft.EntityFrameworkCore;
using assignment3.Entities;

namespace assignment3.Data;

public class AarhusSpaceContext : DbContext
{
    public AarhusSpaceContext(DbContextOptions<AarhusSpaceContext> options)
        : base(options)
    {
    }

    // DbSets – Tabeller
    public DbSet<Staff> Staff {get; set;} = null!;
    public DbSet<Manager> Managers {get; set;} = null!;
    public DbSet<Astronaut> Astronauts {get; set;} = null!;
    public DbSet<LaunchPad> LaunchPads {get; set;} = null!;
    public DbSet<Bodies> Bodies {get; set;} = null!;
    public DbSet<Mission> Missions {get; set;} = null!;
    public DbSet<Rocket> Rockets {get; set;} = null!;
    public DbSet<Scientist> Scientists {get; set;} = null!;


    // Relationer
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Staff
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
        // 1:N
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.ManagedBy)
            .WithMany(ma => ma.ManageMission);   

        // N:N
        modelBuilder.Entity<Mission>()
            .HasMany(m => m.Crew)
            .WithMany(c => c.AstronautMission);

        // N:N
        modelBuilder.Entity<Mission>()
            .HasMany(m => m.Scientists)
            .WithMany(s => s.ScientistMission);

         // 1:N
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.LaunchPad)
            .WithMany(l => l.MissionDetails); 

        // 1:1
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Rocket)
            .WithOne(r => r.RocketUsed)
            .HasForeignKey<Mission>(m => m.RocketId);

         // 1:N
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.CelestialBody)
            .WithMany(b => b.TargetBody);

        modelBuilder.Entity<Bodies>()
            .HasOne(b => b.ParentPlanet)        // En krop (måne) har én forælder
            .WithMany(p => p.Moons)             // En forælder (planet) har mange måner
            .HasForeignKey(b => b.ParentPlanetName) // string-navnet som nøgle
            .OnDelete(DeleteBehavior.Restrict); // Hindrer at sletning af en planet sletter alt ved en fejl
    















    }


}