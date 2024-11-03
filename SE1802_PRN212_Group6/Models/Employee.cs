using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE1802_PRN212_Group6.Models
{
    public class Employee : BaseEntity
    {
        [Required(ErrorMessage = "Image is required")]
        [MaxLength(255, ErrorMessage = "Image can't exceed 255 characters")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        [MaxLength(255, ErrorMessage = "FullName can't exceed 255 characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "IdentificationCard is required")]
        [Phone(ErrorMessage = "IdentificationCard must only contain numbers")]
        [Length(12, 12, ErrorMessage = "IdentificationCard must be 10 characters")]
        [Column(TypeName = "char(12)")]
        public string? IdentificationCard { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Phone must only contain numbers")]
        [Length(10, 10, ErrorMessage = "Phone must be 10 characters")]
        [Column(TypeName = "char(10)")]
        public string? Phone { get; set; }

        public DateOnly Birthday { get; set; }

        public bool Gender { get; set; } = false;

        [Range(1, double.MaxValue, ErrorMessage = "Salary must greater than 0")]
        public double Salary { get; set; }
    }
}
