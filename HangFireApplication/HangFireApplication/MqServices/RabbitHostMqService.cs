
using MassTransit;

namespace HangFireApplication.MqServices
{
    public class RabbitMqHostService : BackgroundService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<RabbitMqHostService> _logger;

        public RabbitMqHostService(IBusControl busControl, ILogger<RabbitMqHostService> logger)
        {
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMqHostService is starting");

            try
            {
                await _busControl.StartAsync(stoppingToken);
                _logger.LogInformation("RabbitMqHostService started successfully");
                await Task.Delay(Timeout.Infinite, stoppingToken);

            }
            catch (Exception)
            {
                _logger.LogError("RabbitMqHostService failed to start");
                throw;

            }
            finally
            {
                await _busControl.StopAsync(stoppingToken);
                _logger.LogInformation("RabbitMqHostService stopped");
            }
        }
    }
}
