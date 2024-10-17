namespace ProductApi.Data.Models
{
    public class ProductVersionsDAO
    {
        public Guid ID { get; set; }
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }

        public Guid? ProductVersionID { get; set; }
        public Guid? ProductID { get; set; }
        public string? ProductVersionName { get; set; }
        public string? ProductVersionDescription { get; set; }

        public DateTime? CreatingDate { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
    }
}
