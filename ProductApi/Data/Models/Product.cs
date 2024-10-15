using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Data.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<ProductVersion>? ProductVersions { get; set; }
    }
}
