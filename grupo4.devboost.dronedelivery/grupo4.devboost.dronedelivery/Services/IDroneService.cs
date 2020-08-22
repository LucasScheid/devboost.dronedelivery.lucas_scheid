using grupo4.devboost.dronedelivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public interface IDroneService
    {
        Task<IEnumerable<Drone>> GetAll();
        Task<List<DronesPedidosDTO>> GetStatusDrone(string connectionString);
    }
}
