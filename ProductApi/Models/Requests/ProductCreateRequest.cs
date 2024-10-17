namespace ProductApi.Models.Requests
{
    public class ProductCreateRequest
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public List<ProductVersionCreateRequest>? ProductVersions { get; set; }
    }
}
