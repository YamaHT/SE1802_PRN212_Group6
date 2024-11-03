using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Models
{
    public class Voucher : TrackableEntity
    {
        [Required(ErrorMessage = "Image is required")]
        [MaxLength(255, ErrorMessage = "Image can't exceed 255 characters")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Name can't exceed 255 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
        public string? Description { get; set; }

        [Range(1, 50, ErrorMessage = "Percentage must be between 1 and 50")]
        [Column(TypeName = "numeric(3, 2)")]
        public double ReducedPercent { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Reducing price must be greater than 0")]
        public double MaxReducing { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        public DateOnly ExpiredDate { get; set; }
    }
}
