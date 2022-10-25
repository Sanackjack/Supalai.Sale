using ClassifiedAds.Infrastructure.DistributedTracing;
using ClassifiedAds.Infrastructure.Web.Filters;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

//using Polly;

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
            
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuer = true,
                        ValidIssuer = AppSettings.JwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience =AppSettings.JwtSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


}

