namespace ProductApi.Models.Requests
{
    public class ProductUpdateRequest
    {
        public Guid? ID { get; set; }
        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}
