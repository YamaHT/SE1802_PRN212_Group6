using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Models
{
    public class Category : BaseEntity
    {
        [MaxLength(255)]
        public required string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
