using MongoDB.Driver;
using System.Text.Json;


namespace MissionLogService;

public class Worker : BackgroundService
{
    // Dependencies injected via constructor
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMongoCollection<MissionLog> _logsCollection;

    // Sample log messages to simulate telemetry data
    private static readonly string[] LogMessages = new[]
    {
        "Telemetry check completed",
        "System diagnostics nominal",
        "Fuel levels within expected range",
        "Crew vitals normal",
        "Communication link established",
        "Navigation systems online",
        "Solar panel output optimal"
    };

    // Constructor to initialize dependencies and MongoDB connection
    public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;

        // Connect to MongoDB
        var mongoUrl = config["MongoDB:ConnectionString"]!;
        var dbName = config["MongoDB:DatabaseName"]!;
        var client = new MongoClient(mongoUrl);
        var db = client.GetDatabase(dbName);
        _logsCollection = db.GetCollection<MissionLog>("MissionLogs");
    }

    // Main execution loop of the worker
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                // 1. Call the Web API to get active missions
                var httpClient = _httpClientFactory.CreateClient("WebApi");
                var response = await httpClient.GetAsync("api/missions?status=Active", stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(stoppingToken);
                    var missions = JsonSerializer.Deserialize<List<MissionDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (missions != null && missions.Count > 0)
                    {
                        var random = new Random();
                        foreach (var mission in missions)
                        {
                            var log = new MissionLog
                            {
                                MissionId = mission.MissionId,
                                Message = LogMessages[random.Next(LogMessages.Length)],
                                Timestamp = DateTime.UtcNow
                            };

                            await _logsCollection.InsertOneAsync(log, null, stoppingToken);
                            _logger.LogInformation("Logged mission {MissionId}: {Message}", log.MissionId, log.Message);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No active missions found.");
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to fetch missions. Status: {Status}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing mission logs.");
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}