using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Connection String
string connectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrongP@ssword123!;TrustServerCertificate=True";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
var app = builder.Build();

app.MapOpenApi("/openapi/v1.json");
app.MapScalarApiReference(options => { options.Title = "Aarhus Space Program API"; });

// Query 1: Mission Overview
app.MapGet("/api/missions/overview", () =>
{
    var missions = new List<object>();

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = @"
            SELECT m.name, m.launch_date, s.name AS manager, r.model_name AS rocket, m.destination_name
            FROM dbo.Mission m
            JOIN dbo.Staff s ON m.manager_id = s.staff_id
            LEFT JOIN dbo.Rocket r ON m.rocket_serial = r.serial_number";

        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                missions.Add(new {
                    Name = reader["name"].ToString(),
                    LaunchDate = reader["launch_date"],
                    Manager = reader["manager"].ToString(),
                    Rocket = reader["rocket"].ToString(),
                    Destination = reader["destination_name"].ToString()
                });
            }
        }
    }
    return missions;
});

// Query 2: Mission crew list (Manager, Astronauts, Scientists)
app.MapGet("/api/missions/{name}/crew", (string name) =>
{
    var crew = new List<object>();
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = @"
            SELECT s.name, 'Manager' as role FROM dbo.Staff s JOIN dbo.Mission m ON s.staff_id = m.manager_id WHERE m.name = @name
            UNION
            SELECT s.name, 'Astronaut' FROM dbo.Staff s JOIN dbo.Astronaut_Mission am ON s.staff_id = am.staff_id WHERE am.mission_name = @name
            UNION
            SELECT s.name, 'Scientist' FROM dbo.Staff s JOIN dbo.Scientist_Mission sm ON s.staff_id = sm.staff_id WHERE sm.mission_name = @name";

        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", name); // Security: Use parameters!
        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                crew.Add(new { Name = reader["name"].ToString(), Role = reader["role"].ToString() });
            }
        }
    }
    return crew;
});

// Query 3: Active or upcoming missions
app.MapGet("/api/missions/upcoming", () =>
{
    var missions = new List<object>();
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = "SELECT name, launch_date, status FROM dbo.Mission WHERE status IN ('Active', 'Planned', 'Scheduled')";
        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                missions.Add(new { Name = reader["name"].ToString(), Date = reader["launch_date"], Status = reader["status"].ToString() });
            }
        }
    }
    return missions;
});

// Query 4: Astronauts per mission (Detailed List)
app.MapGet("/api/missions/astronauts", () =>
{
    var list = new List<object>();
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = @"
            SELECT am.mission_name, s.name 
            FROM dbo.Astronaut_Mission am 
            JOIN dbo.Staff s ON am.staff_id = s.staff_id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new { Mission = reader["mission_name"].ToString(), Astronaut = reader["name"].ToString() });
            }
        }
    }
    return list;
});

// Query 5: Astronaut count per mission
app.MapGet("/api/missions/crew-count", () =>
{
    var stats = new List<object>();
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = "SELECT mission_name, COUNT(staff_id) as total FROM dbo.Astronaut_Mission GROUP BY mission_name";
        SqlCommand cmd = new SqlCommand(sql, conn);
        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                stats.Add(new {
                    Mission = reader["mission_name"].ToString(),
                    Count = reader["total"]
                });
            }
        }
    }
    return stats;
});

// Query 6: Total weight launched from a specific launchpad
app.MapGet("/api/launchpad/{location}/weight", (string location) =>
{
    object totalWeight = 0;
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string sql = @"
            SELECT SUM(r.weight) as total 
            FROM dbo.Mission m 
            JOIN dbo.Rocket r ON m.rocket_serial = r.serial_number 
            WHERE m.launchpad_location = @location";
        
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@location", location);
        conn.Open();
        totalWeight = cmd.ExecuteScalar() ?? 0; // Use ExecuteScalar for single values
    }
    return new { Launchpad = location, TotalWeight = totalWeight };
});


app.Run();