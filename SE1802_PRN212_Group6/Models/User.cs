using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Email can't exceed 50 characters")]
        [EmailAddress(ErrorMessage = "Email must be in correct format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and no longer than 16 characters")]
        public string? Password { get; set; }

        public bool Role { get; set; } = false;

        public virtual ICollection<Booking> Bookings { get; set; } = [];
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}
