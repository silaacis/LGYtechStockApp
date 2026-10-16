using System;

namespace LGYtechStockApp.ViewModels
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderLineCount { get; set; }
        public decimal Total { get; set; }
    }
}