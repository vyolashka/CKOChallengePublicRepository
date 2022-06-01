using DbAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using DbAccess.Interfaces;
using DbAccess.Repositories;
using Services.Interfaces;
using Services.Services;
using Services.BankSimulator;
using PaymentGateway.CustomExceptionMiddleware;

namespace PaymentGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContextPool<PaymentGatewayDbContext>((provider, options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PaymentGatewayDbConnectionString"));
                options.UseInternalServiceProvider(provider);
            });

            services.AddEntityFrameworkSqlServer();

            // Use NewtonsoftJson instead of System.Text.Json, because the latter does not handle parsing of 
            // nullable types
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "dd.MM.yyyy";
            }
            );

            services.AddScoped(typeof(IAllRepositories), typeof(AllRepositories));
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBankSimulator, BankSimulator>();
            services.AddScoped<IPaymentValidationService, PaymentValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}
