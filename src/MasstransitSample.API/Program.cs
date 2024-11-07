using Common;
using MassTransit;
using MasstransitSample.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddDelayedMessageScheduler();

    busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));

    busConfigurator.UsingRabbitMq((busContext, rabbitMqBusConfigurator) =>
    {
        var settings = busContext.GetRequiredService<IConfiguration>().GetSection("RabbitmqSettings").Get<RabbitmqSettings>()!;

        rabbitMqBusConfigurator.Host(settings.Host,settings.Port, "/" , hostConfigurator =>
        {
            hostConfigurator.Username(settings.Username);
            hostConfigurator.Password(settings.Password);
        });

        rabbitMqBusConfigurator.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());


        rabbitMqBusConfigurator.Message<ProductCreated>(configTopology =>
        {
            string name = KebabCaseEndpointNameFormatter.Instance.SanitizeName(configTopology.EntityName);
            configTopology.SetEntityName(name);
        });



        rabbitMqBusConfigurator.Publish<PriceChanged>(topology =>
        {
            string queueName = KebabCaseEndpointNameFormatter.Instance.SanitizeName(topology.Exchange.ExchangeName + "-queue");
            topology.BindQueue(topology.Exchange.ExchangeName, queueName);
        });

        //rabbitMqBusConfigurator.ReceiveEndpoint("product-created", configureEndpoint =>
        //{
        //    configureEndpoint.BindQueue = true;
        //});

        //rabbitMqBusConfigurator.ConfigureEndpoints(busContext);
    });
});

var app = builder.Build();

app.MapGet("/", () => "it works");

app.MapGet("publish", async (IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
{
    PriceChanged priceChanged = new PriceChanged() { NewPrice = 12, OldPrice = 9 };
    await publishEndpoint.Publish(priceChanged, cancellationToken);

    return Results.Ok("message published !");
});


app.MapGet("send", async (IBus bus, CancellationToken cancellationToken) =>
{
    ProductCreated productCreated = new ProductCreated();
    var endpoint = await bus.GetSendEndpoint(new Uri("queue:product-created"));
    await endpoint.Send(productCreated, cancellationToken);

    return Results.Ok("message sended !");
});

app.Run();
