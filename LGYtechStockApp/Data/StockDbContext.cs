using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LGYtechStockApp.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext() : base("name=StockDbContext")
        {
        }
    }
}