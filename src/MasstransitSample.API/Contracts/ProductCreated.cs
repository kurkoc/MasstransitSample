namespace MasstransitSample.API.Contracts
{
    public class ProductCreated
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; } = 3;
    }
}
