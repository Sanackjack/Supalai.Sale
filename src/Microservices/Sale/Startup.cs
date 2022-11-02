﻿using ClassifiedAds.Infrastructure.DistributedTracing;
using ClassifiedAds.Infrastructure.Web.Filters;
using ClassifiedAds.Infrastructure.Middleware;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("ar"),
                        new CultureInfo("th")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
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

            services.AddDistributedTracing(AppSettings.DistributedTracing);

            services.AddDateTimeProvider();
            services.AddApplicationServices();

            services.AddHtmlGenerator();
            services.AddDinkToPdfConverter();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSwaggerGen();

            // services.AddProductModule(AppSettings);
            // services.AddHostedServicesProductModule();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.Authority = AppSettings.IdentityServerAuthentication.Authority;
            //            options.Audience = AppSettings.IdentityServerAuthentication.ApiName;
            //            options.RequireHttpsMetadata = AppSettings.IdentityServerAuthentication.RequireHttpsMetadata;
            //        });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAppLogger, AppLogger>();


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

            app.UseRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowAnyOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }


}

