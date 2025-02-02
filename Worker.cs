using System.Diagnostics;

namespace PerformanceMonitor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private PerformanceCounter _cpuCount;
        private PerformanceCounter _memoryCount;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _cpuCount = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _memoryCount = new PerformanceCounter("Memory", "Available MBytes");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Performance Monitoring Service started");
            while (!stoppingToken.IsCancellationRequested)
            {
                float cpuValue = _cpuCount.NextValue();
                if(cpuValue>45)
                {
                    _logger.LogWarning($"CPU usages is more that {cpuValue}");
                }
                float memoryValue = _memoryCount.NextValue();

                _logger.LogInformation($"CPU Usage: {cpuValue:F2}% | Available Memory: {memoryValue:F2} MB");
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Performance Monitorin Service stopped");
            _cpuCount.Dispose();
            _memoryCount.Dispose();
            await base.StopAsync(stoppingToken);
        }
    }
}
