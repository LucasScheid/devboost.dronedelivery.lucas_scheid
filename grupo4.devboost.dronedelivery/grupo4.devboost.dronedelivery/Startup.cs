using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace grupo4.devboost.dronedelivery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IPedidoService, PedidoService>();
            services.AddSingleton<IDroneService, DroneService>();

            services.AddDbContext<grupo4devboostdronedeliveryContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("grupo4devboostdronedeliveryContext")), ServiceLifetime.Singleton);
        }

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
