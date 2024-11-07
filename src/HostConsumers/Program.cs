using Common;
using HostConsumers.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddDelayedMessageScheduler();
    busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));

    busConfigurator.UsingRabbitMq((busContext, rabbitMqBusConfigurator) =>
    {
        var settings = busContext.GetRequiredService<IConfiguration>().GetSection("RabbitmqSettings").Get<RabbitmqSettings>()!;

        rabbitMqBusConfigurator.Host(settings.Host, settings.Port, "/", hostConfigurator =>
        {
            hostConfigurator.Username(settings.Username);
            hostConfigurator.Password(settings.Password);
        });

        rabbitMqBusConfigurator.ReceiveEndpoint("product-created", configureEndpoint =>
        {
            configureEndpoint.ConfigureConsumeTopology = false;
            configureEndpoint.Consumer<ProductCreatedConsumer>();
        });
    });
});

var host = builder.Build();
host.Run();
