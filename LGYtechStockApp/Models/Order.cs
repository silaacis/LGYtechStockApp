using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGYtechStockApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}