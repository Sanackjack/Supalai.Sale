using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.Repositories;

namespace Spl.Crm.SaleOrder;

public static class SaleOrderModuleServiceCollectionExtensions
{
    public static IServiceCollection AddSaleOrderModule(this IServiceCollection services, AppSettings appSettings)
    {
        // services.AddDbContext<SaleOrderDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.ClassifiedAds, sql =>
        // {
        //     if (!string.IsNullOrEmpty(appSettings.ConnectionStrings.MigrationsAssembly))
        //     {
        //         sql.MigrationsAssembly(appSettings.ConnectionStrings.MigrationsAssembly);
        //     }
        // }));
        
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<ISysAdminUserRepository, SysAdminUserRepository>();

        services.AddDbContext<SaleOrderDbContext>(options =>
            options.UseSqlServer(
                appSettings.ConnectionStrings.ClassifiedAds,
                b => b.MigrationsAssembly(typeof(SaleOrderDbContext).Assembly.FullName)));
        return services;

        
    }
}