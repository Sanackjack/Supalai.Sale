using ClassifiedAds.Infrastructure.Caching;
using ClassifiedAds.Infrastructure.DistributedTracing;
using ClassifiedAds.Infrastructure.Interceptors;
using ClassifiedAds.Infrastructure.Logging;
using ClassifiedAds.Infrastructure.MessageBrokers;
using ClassifiedAds.Infrastructure.Monitoring;
using ClassifiedAds.Infrastructure.Storages;

namespace Spl.Crm.SaleOrder.ConfigurationOptions
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public LoggingOptions Logging { get; set; }

        public CachingOptions Caching { get; set; }

        public MonitoringOptions Monitoring { get; set; }
        public DistributedTracingOptions DistributedTracing { get; set; }
        
        public StorageOptions Storage { get; set; }

        public MessageBrokerOptions MessageBroker { get; set; }

        public InterceptorsOptions Interceptors { get; set; }
        
        public JwtSettings JwtSettings { get; set; }
        
        public LDAPSettings LDAPSettings { get; set; }
    }

    public class ConnectionStrings
    {
        public string ClassifiedAds { get; set; }

        public string MigrationsAssembly { get; set; }
    }

    public class JwtSettings{
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Expire { get; set; }
    }
    public class LDAPSettings{
        public string LDAPHost { get; set; }
        public string LDAPPath { get; set; }
        public string LDAPDomain { get; set; }
    }
}

