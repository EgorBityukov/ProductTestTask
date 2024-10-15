namespace ProductApi.Models.DTO
{
    public class ProductDTO
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<ProductVersionDTO>? ProductVersions { get; set; }
    }
}
