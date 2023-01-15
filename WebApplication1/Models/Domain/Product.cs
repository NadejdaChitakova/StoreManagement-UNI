namespace WebApplication1.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int ProductCount { get; set; }
        public string PathFile { get; set; }
        public string CategoryId { get; set; }
        public decimal ProductCode { get; set; }
    }
}
