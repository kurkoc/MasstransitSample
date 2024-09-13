using MassTransit;
using MasstransitSample.API.Contracts;

namespace MasstransitSample.API.Consumers
{
    public class PriceChangedConsumer : IConsumer<PriceChanged>
    {
        public async Task Consume(ConsumeContext<PriceChanged> context)
        {
            var message = context.Message;

            Console.WriteLine($"price changed to {message.NewPrice} from {message.OldPrice}");

            await Task.CompletedTask;
        }
    }
}
