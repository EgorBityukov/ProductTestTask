namespace ProductApplication.Models
{
    public class ProductVersionViewModel
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
    }
}
