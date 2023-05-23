using DataAccess.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using MvcWebUI.Identity;

namespace MvcWebUI.Extensions
{
  public static class MigrationManager
  {
    //Extensions metodu kullanırken this mutlaka kullanılmalı
    public static IHost MigrateDatabase(this IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
        {
          try//kullanmamızın sebebi bir hata varsa uygulama durmasın ht gösterilsin
          {
            applicationContext.Database.Migrate();
          }
          catch (Exception)
          {

            throw;
          }
        }
        using (var shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>())
        {
          try
          {
            shopContext.Database.Migrate();
          }
          catch (Exception)
          {

            throw;
          }
        }
      }
      
      return host;
    }
  }
}
