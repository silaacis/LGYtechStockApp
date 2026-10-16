using LGYtechStockApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LGYtechStockApp.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [StringLength(100)]
        public string CustomerName { get; set; }

        public List<OrderLineViewModel> OrderLines { get; set; } = new List<OrderLineViewModel>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}