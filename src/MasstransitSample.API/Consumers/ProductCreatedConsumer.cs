using MassTransit;

namespace MasstransitSample.API.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;

            Console.WriteLine($"product created : {message.Id} - Adet : {message.Quantity}");

            await Task.CompletedTask;

        }
    }
}
