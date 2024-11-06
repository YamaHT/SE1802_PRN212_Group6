using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE1802_PRN212_Group6.Models
{
    public class Order : BaseEntity
    {
        public int Quantity { get; set; }

        public double Total { get; set; }

        public double ActualPayment { get; set; }

        public DateOnly? OrderDate { get; set; } = null;

        [MaxLength(255, ErrorMessage = "RecipientName can't exceed 255 characters")]
        public string? RecipientName { get; set; }

        [MaxLength(255, ErrorMessage = "Address can't exceed 255 characters")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Phone must only contain numbers")]
        [Length(10, 10, ErrorMessage = "Phone must be 10 characters")]
        [Column(TypeName = "char(10)")]
        public string? Phone { get; set; }

        public virtual User? User { get; set; }

        public virtual Voucher? Voucher { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}
