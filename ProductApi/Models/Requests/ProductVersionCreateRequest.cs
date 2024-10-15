namespace ProductApi.Models.Requests
{
    public class ProductVersionCreateRequest
    {
        public Guid ProductID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
    }
}
