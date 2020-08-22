using Microsoft.EntityFrameworkCore;

namespace grupo4.devboost.dronedelivery.Data
{
    public class grupo4devboostdronedeliveryContext : DbContext
    {
        public grupo4devboostdronedeliveryContext(DbContextOptions<grupo4devboostdronedeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<grupo4.devboost.dronedelivery.Models.Pedido> Pedido { get; set; }

        public DbSet<grupo4.devboost.dronedelivery.Models.Drone> Drone { get; set; }
    }
}
