using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LGYtechStockApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Sku { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative value.")]
        public int Stock { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}