using Autofac;
using Autofac.Integration.Mvc;
using LGYtechStockApp.Data;
using System.Web.Mvc;

namespace LGYtechStockApp.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            // 1. ContainerBuilder örneği oluşturulur
            var builder = new ContainerBuilder();

            // 2. MVC Controller sınıfları kaydedilir (Tüm Controller'lar IoC container'a tanıtılır)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // 3. Kendi servis ve repository sınıflarınız arayüzleriyle eşleştirilir
            builder.RegisterType<StockDbContext>()
                .AsSelf()
                .InstancePerRequest(); // Her HTTP isteği için yeni bir StockDbContext örneği oluşturulur

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); // MVC için DependencyResolver ayarlanır
        }
    }
}