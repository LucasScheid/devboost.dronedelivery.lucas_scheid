using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using grupo4.devboost.dronedelivery.Models;

namespace grupo4.devboost.dronedelivery.Data
{
    public class grupo4devboostdronedeliveryContext : DbContext
    {
        public grupo4devboostdronedeliveryContext (DbContextOptions<grupo4devboostdronedeliveryContext> options)
            : base(options)
        {
        }

        public DbSet<grupo4.devboost.dronedelivery.Models.Pedido> Pedido { get; set; }

        public DbSet<grupo4.devboost.dronedelivery.Models.Drone> Drone { get; set; }
    }
}
