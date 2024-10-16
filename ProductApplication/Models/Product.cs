namespace ProductApplication.Models
{
    public class Product
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
