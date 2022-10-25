using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore;

namespace Spl.Crm.SaleOrder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseClassifiedAdsLogger(configuration =>
                {
                    return new LoggingOptions();
                });
    }

}