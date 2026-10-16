using LGYtechStockApp.Models;
using System.Collections.Generic;

namespace LGYtechStockApp.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10; // Default page size
        public int TotalCount { get; set; }
    }
}