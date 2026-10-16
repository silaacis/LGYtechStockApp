using LGYtechStockApp.Data;
using LGYtechStockApp.Models;
using LGYtechStockApp.ViewModels;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LGYtechStockApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly StockDbContext _context;
        public OrdersController(StockDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [HttpGet]
        public async Task<ActionResult> OrderList()
        {
            var values = await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderListViewModel
                {
                    OrderId = o.Id,
                    CustomerName = o.CustomerName,
                    CreatedAt = o.CreatedAt,
                    OrderLineCount = o.OrderLines.Count(),
                    Total = o.Total
                }).ToListAsync();

            return View(values);
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrder()
        {
            var model = new CreateOrderViewModel
            {
                Products = await _context.Products.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (model.OrderLines == null || !model.OrderLines.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Siparişe en az bir ürün eklemelisiniz."
                });
            }

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Müşteri bilgilerini ve ürün adetlerini kontrol edin."
                });
            }

            // Aynı ürün birden fazla satırda seçilmişse adetlerini birleştir.
            var requestedItems = model.OrderLines
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Ürünlerin veritabanından Idlerini aldık.
                    var productIds = requestedItems
                        .Select(x => x.ProductId)
                        .ToList();

                    // Seçilen ürünlerin hepsinin veritabanından çektik.
                    var products = await _context.Products
                        .Where(p => productIds.Contains(p.Id))
                        .ToListAsync();

                    // Eğer ürünlerden biri bulunamazsa, işlem geri alınır ve hata mesajı döner.
                    if (products.Count != productIds.Distinct().Count())
                    {
                        transaction.Rollback();

                        return Json(new
                        {
                            success = false,
                            message = "Seçilen ürünlerden biri bulunamadı."
                        });
                    }

                    // İstenen toplam adetleri mevcut stokla karşılaştır.
                    foreach (var item in requestedItems)
                    {
                        var product = products
                            .First(p => p.Id == item.ProductId);

                        if (product.Stock < item.Quantity)
                        {
                            transaction.Rollback();

                            return Json(new
                            {
                                success = false,
                                message = product.Name +
                                    " için yeterli stok yok. " +
                                    "Mevcut stok: " + product.Stock +
                                    ", istenen adet: " + item.Quantity + "."
                            });
                        }
                    }

                    // Fiyatları sipariş anındaki ürün fiyatlarından hesapla.
                    decimal total = requestedItems.Sum(item =>
                        products.First(p => p.Id == item.ProductId).Price
                        * item.Quantity);

                    // Her stok azaltımı koşullu ve atomiktir.
                    // Eş zamanlı siparişlerde stok eksiye düşmez.
                    foreach (var item in requestedItems)
                    {
                        int affectedRows = await _context.Database
                            .ExecuteSqlCommandAsync(
                                @"UPDATE dbo.Products
                          SET Stock = Stock - @quantity
                          WHERE Id = @productId
                            AND Stock >= @quantity",
                                new SqlParameter("@quantity", item.Quantity),
                                new SqlParameter("@productId", item.ProductId));

                        // Eğer güncelleme sırasında etkilenen ürün sayısı 1 değilse, bu stok değiştiği anlamına gelir ve işlem geri alınır.
                        if (affectedRows != 1)
                        {
                            transaction.Rollback();

                            return Json(new
                            {
                                success = false,
                                message = "Stok sipariş sırasında değişti. " +
                                          "Lütfen güncel stokla tekrar deneyin."
                            });
                        }
                    }

                    var order = new Order
                    {
                        CustomerName = model.CustomerName.Trim(),
                        CreatedAt = DateTime.Now,
                        Total = total
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in requestedItems)
                    {
                        var product = products
                            .First(p => p.Id == item.ProductId);

                        _context.OrderLines.Add(new OrderLine
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price
                        });

                        _context.StockMovements.Add(new StockMovement
                        {
                            ProductId = item.ProductId,
                            Change = -item.Quantity,
                            Reason = "Sipariş #" + order.Id,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        message = "Sipariş başarıyla oluşturuldu.",
                        orderId = order.Id
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    System.Diagnostics.Trace.TraceError(
                        "Sipariş oluşturulurken hata oluştu: " + ex);

                    return Json(new
                    {
                        success = false,
                        message = "Sipariş kaydedilemedi. Lütfen tekrar deneyin."
                    });
                }
            }
        }
    }
}