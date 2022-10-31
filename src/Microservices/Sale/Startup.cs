using ClassifiedAds.Infrastructure.DistributedTracing;
using ClassifiedAds.Infrastructure.Web.Filters;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using ClassifiedAds.Infrastructure.Web.Middleware;
using Microsoft.AspNetCore.Mvc;


namespace Spl.Crm.SaleOrder
{
	public class Startup
	{
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            AppSettings = new AppSettings();
            Configuration.Bind(AppSettings);
        }

        public IConfiguration Configuration { get; }

        private AppSettings AppSettings { get; set; }


        // This method gets called by the runtime.Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           // AppSettings.ConnectionStrings.MigrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
           services.AddApiVersioning(config =>
           {
               config.DefaultApiVersion = new ApiVersion(1, 0);
               config.AssumeDefaultVersionWhenUnspecified = true;
               config.ReportApiVersions = true;
           });
           
            services.AddControllers(configure =>
            {
                configure.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            // configure strongly typed settings object
           /// services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            services.AddDistributedTracing(AppSettings.DistributedTracing);

            services.AddDateTimeProvider();
            services.AddApplicationServices();

            services.AddHtmlGenerator();
            services.AddDinkToPdfConverter();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<ILDAPUtils, LDAPUtils>();
            services.AddScoped<IAuthService, AuthService>();
            

            services.AddSwaggerGen();
            services.AddSaleOrderModule(AppSettings);
            // services.AddProductModule(AppSettings);
            // services.AddHostedServicesProductModule();
            
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddDaprClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Policy.Handle<Exception>().WaitAndRetry(new[]
            //{
            //    TimeSpan.FromSeconds(10),
            //    TimeSpan.FromSeconds(20),
            //    TimeSpan.FromSeconds(30),
            //})
            //.Execute(() =>
            //{
            //    app.MigrateProductDb();
            //});

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowAnyOrigin");

            // app.UseAuthentication();
            // app.UseAuthorization();
            
            // global error handler
            //app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseGlobalExceptionHandlerMiddleware();
            app.UseMiddleware<JwtMiddleware>();
            
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


}

