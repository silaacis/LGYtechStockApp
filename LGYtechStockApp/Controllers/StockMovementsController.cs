using LGYtechStockApp.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LGYtechStockApp.Controllers
{
    public class StockMovementsController : Controller
    {
        private readonly StockDbContext _context;
        public StockMovementsController(StockDbContext context)
        {
            _context = context;
        }

        // GET: StockMovements
        public async Task<ActionResult> Index(int? productId)
        {
            ViewBag.Products = await _context.Products
                .OrderBy(p=> p.Name)
                .ToListAsync();

            var query = _context.StockMovements
                .Include(sm => sm.Product)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(sm => sm.ProductId == productId.Value);
            }
            
            var stockMovements = await query
                .OrderByDescending(sm => sm.CreatedAt)
                .ToListAsync();

            ViewBag.SelectedProductId = productId;

            return View(stockMovements);
        }
    }
}