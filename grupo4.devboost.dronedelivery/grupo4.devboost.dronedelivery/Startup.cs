using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grupo4.devboost.dronedelivery.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Services;

namespace grupo4.devboost.dronedelivery
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

            services.AddSingleton<IPedidoService, PedidoService>();
            services.AddSingleton<IDroneService, DroneService>();

            services.AddDbContext<grupo4devboostdronedeliveryContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("grupo4devboostdronedeliveryContext")),ServiceLifetime.Singleton);
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
