using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE1802_PRN212_Group6.Models
{
    public class Booking : BaseEntity
    {
        [Required(ErrorMessage = "FullName is required")]
        [MaxLength(255, ErrorMessage = "FullName can't exceed 255 characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Phone must only contain numbers")]
        [Length(10, 10, ErrorMessage = "Phone must be 10 characters")]
        [Column(TypeName = "char(10)")]
        public string? Phone { get; set; }

        [MaxLength(1000, ErrorMessage = "Note can't exceed 1000 characters")]
        public string? Note { get; set; }

        public DateOnly BookingDate { get; set; }
        
        public TimeOnly ArrivalTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "People must be greater than 0")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "User is required")]
        public virtual User? User { get; set; }

        [Required(ErrorMessage = "Table is required")]
        public virtual Table? Table { get; set; }
    }
}
