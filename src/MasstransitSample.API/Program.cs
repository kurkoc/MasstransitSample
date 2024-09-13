using MassTransit;
using MasstransitSample.API.Consumers;
using MasstransitSample.API.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddDelayedMessageScheduler();
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<PriceChangedConsumer>();

    busConfigurator.UsingRabbitMq((busContext, rabbitMqBusConfigurator) =>
    {
        var settings = busContext.GetRequiredService<IConfiguration>().GetSection("RabbitmqSettings").Get<RabbitmqSettings>()!;

        rabbitMqBusConfigurator.Host(settings.Host, host =>
        {
            host.Username(settings.Username);
            host.Password(settings.Password);
        });

        rabbitMqBusConfigurator.ReceiveEndpoint("product-created", receiveEndpointConfigurator =>
        {
            receiveEndpointConfigurator.Consumer<ProductCreatedConsumer>();
        });

        rabbitMqBusConfigurator.ConfigureEndpoints(busContext);
    });
});

var app = builder.Build();

app.MapGet("/", () => "it works");

app.MapGet("change", async (IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
{
    PriceChanged priceChanged = new PriceChanged() { NewPrice = 12, OldPrice = 9 };
    await publishEndpoint.Publish(priceChanged, cancellationToken);

    return Results.Ok();
});


app.MapGet("send", async (IBus bus, CancellationToken cancellationToken) =>
{
    ProductCreated productCreated = new ProductCreated();
    var endpoint = await bus.GetSendEndpoint(new Uri("exchange:product-created"));
    await endpoint.Send(productCreated, cancellationToken);

    return Results.Ok();
});

app.Run();
