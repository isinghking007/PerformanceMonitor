using PerformanceMonitor;
using Serilog;
using System.Configuration;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Set up Serilog directly
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
var host = builder.Build();
host.Run();
