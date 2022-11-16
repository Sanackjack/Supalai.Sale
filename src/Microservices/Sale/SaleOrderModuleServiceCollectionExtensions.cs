using ClassifiedAds.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using ClassifiedAds.Domain.Uow;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories.Impl;
using IUnitOfWork = ClassifiedAds.Domain.Uow.IUnitOfWork;

namespace Spl.Crm.SaleOrder;

public static class SaleOrderModuleServiceCollectionExtensions
{
    public static IServiceCollection AddSaleOrderModule(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<ISysAdminUserRepository, SysAdminUserRepository>();
        services.AddTransient<ISysAdminRoleRepository, SysAdminRoleRepository>();
        services.AddTransient<IAuthCustomRepository, AuthCustomRepository>();
        services.AddTransient<IProjectRepository, ProjectRepository>();

        services.AddDbContext<SaleOrderDBContext>(options =>
            options.UseSqlServer(
                appSettings.ConnectionStrings.ClassifiedAds,
                b => b.MigrationsAssembly(typeof(SaleOrderDBContext).Assembly.FullName))
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning)));
       
        services.AddScoped<IUnitOfWork, UnitOfWork<SaleOrderDBContext>>();
        return services;

        
    }
}