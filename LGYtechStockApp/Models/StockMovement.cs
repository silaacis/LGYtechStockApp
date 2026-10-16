using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGYtechStockApp.Models
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Change { get; set; }
        [Required]
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Product Product { get; set; }
    }
}