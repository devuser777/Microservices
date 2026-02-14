using InventoryService.Data;
using InventoryService.RedisCache;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService
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

            //Receiver -- > Consumer
            services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderConsumer>();

                config.UsingRabbitMq((context, cfg) =>
                {
                    var uri = new Uri("rabbitmq://localhost:5672");
                    cfg.Host(uri, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("orderQ", c =>
                    {
                        c.ConfigureConsumer<OrderConsumer>(context);
                    });
                });


            });

            services.AddMassTransitHostedService();

            services.AddDbContext<InventoryDBContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetConnectionString("EMSDBConnection"));

                options.UseSqlServer(Configuration.GetConnectionString("InventoryDBConnection"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("Redis");
                options.InstanceName = "InventoryApp:";
            });

            services.AddScoped<IInventoryCacheService, InventoryCacheService>();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
