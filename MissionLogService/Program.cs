using MissionLogService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApi:BaseUrl"]!);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();