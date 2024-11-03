using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
