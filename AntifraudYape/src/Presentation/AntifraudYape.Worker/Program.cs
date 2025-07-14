using AntifraudYape.Worker.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var host = builder.Build();
host.Run();

