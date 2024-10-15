using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Data.Models
{
    [Table("ProductVersion")]
    public class ProductVersion
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public Guid ProductID { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime CreatingDate { get; set; }

        [Required]
        public double Width { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Length { get; set; }

        public required Product Product { get; set; }
    }
}
