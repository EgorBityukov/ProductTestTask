using System.ComponentModel.DataAnnotations;

namespace ProductApplication.Models
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ProductVersion>? ProductVersions { get; set; }
    }
}
