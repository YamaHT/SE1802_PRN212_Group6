using SE1802_PRN212_Group6.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Models
{
    public class Table : TrackableEntity
    {
        [Required(ErrorMessage = "Image is required")]
        [MaxLength(255, ErrorMessage = "Image can't exceed 255 characters")]
        public string? Image { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Floor can't be negative")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [MaxLength(20)]
        [EnumDataType(typeof(TypeEnum), ErrorMessage = "This type of table is not available")]
        public string? Type { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = [];
    }
}
