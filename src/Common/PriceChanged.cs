namespace Common
{
    public class PriceChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
    }
}
