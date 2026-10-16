using LGYtechStockApp.Data;
using LGYtechStockApp.Models;
using LGYtechStockApp.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LGYtechStockApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StockDbContext _context;
        public ProductsController(StockDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [HttpGet]
        public async Task<ActionResult> ProductList(string searchTerm, int page = 1)
        {
            page = Math.Max(1, page);
            var pageSize = 10;

            var query = _context.Products.AsQueryable();

            // Filter products based on the search term if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            // sayfalama
            var products = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm
            };

            return View(model);
        }

        //GET: Products/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View(new ProductFormViewModel());
        }

        //POST: Products/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(ProductFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool skuExists = await _context.Products.AnyAsync(p => p.Sku == model.Sku);

                if (skuExists)
                {
                    ModelState.AddModelError("Sku", "Bu SKU zaten kullanılıyor.");
                    return View(model);
                }

                var product = new Product
                {
                    Name = model.Name,
                    Sku = model.Sku,
                    Price = model.Price,
                    Stock = model.Stock
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProductList");
            }
            return View(model);
        }

        //GET: Products/EditProduct/5
        [HttpGet]
        public async Task<ActionResult> EditProduct(int id) 
        { 
            var product = await _context.Products.FindAsync(id); 

            if (product == null) 
            { 
                return HttpNotFound();
            }

            var model = new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                Stock = product.Stock
            };
            return View(model);
        }

        //POST: Products/EditProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(int id, ProductFormViewModel model)
        {
            if (id != model.Id)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            bool skuExists = await _context.Products.AnyAsync(p => p.Sku == model.Sku && p.Id != id);

            if (skuExists)
            {
                ModelState.AddModelError("Sku", "Bu SKU başka bir ürün tarafından kullanılıyor.");
                return View(model);
            }

            product.Name = model.Name;
            product.Sku = model.Sku;
            product.Price = model.Price;
            product.Stock = model.Stock;

            await _context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
    }
}