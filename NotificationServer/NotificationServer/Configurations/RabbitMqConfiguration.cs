using MassTransit;
using Shared.Configurations;
using Shared.Constants;

namespace NotificationServer.Configurations;

public static class RabbitMqConfiguration
{
    public static void ConfigureRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration
            .GetSection("RabbitMq")
            .Get<RabbitMqConfig>();

        services.AddSingleton(rabbitMqConfig);

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig.Host, u =>
                {
                    u.Username(rabbitMqConfig.Username);
                    u.Password(rabbitMqConfig.Password);
                });

                cfg.ReceiveEndpoint(Consts.RabbitMqConsts.HELLOJOB_QUEUE, e =>
                {
                    //e.Consumer<JobSearchConsumer>(context);
                });

                cfg.ReceiveEndpoint(Consts.RabbitMqConsts.JOBSEARCH_QUEUE, e =>
                {
                    //e.Consumer<JobSearchConsumer>(context);
                });
                cfg.ReceiveEndpoint(Consts.RabbitMqConsts.BOSS_AZ_QUEUE, e =>
                {
                    //e.Consumer<JobSearchConsumer>(context);
                });
            });
        });

    }
}

