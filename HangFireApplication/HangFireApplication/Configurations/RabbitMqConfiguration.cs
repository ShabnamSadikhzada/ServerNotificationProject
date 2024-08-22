using HangFireApplication.MqServices;
using MassTransit;

namespace HangFireApplication.Configurations;

public static class RabbitMqConfiguration
{
    public static void ConfigureRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
        services.AddSingleton(rabbitMqConfig);

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig.Host, h =>
                {
                    h.Username(rabbitMqConfig.Username);
                    h.Password(rabbitMqConfig.Password);
                });

                cfg.ReceiveEndpoint("job-search-queue", e =>
                {

                });
            });
        });
        services.AddHostedService<RabbitMqHostService>();
    }
}

public class RabbitMqConfig
{
    public string Host { get; set; }
    public int Port { get; set; } = 5672;
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }

}
