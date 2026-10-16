using System.ComponentModel.DataAnnotations;

namespace LGYtechStockApp.ViewModels
{
    public class OrderLineViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Adet en az 1 olmalıdır.")]
        public int Quantity { get; set; }
    }
}