using System.ComponentModel.DataAnnotations;

namespace LGYtechStockApp.ViewModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SKU zorunludur.")]
        [StringLength(50, ErrorMessage = "SKU 50 karakteri geçemez.")]
        public string Sku { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyat pozitif bir değer olmalıdır.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok negatif olamaz.")]
        public int Stock { get; set; }
    }
}