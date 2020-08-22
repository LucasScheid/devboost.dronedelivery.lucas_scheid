using Geolocation;
using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public class PedidoService : IPedidoService
    {
        private const double LATITUDE_SAIDA_DRONE = -23.5880684;
        private const double LONGITUDE_SAIDA_DRONE = -46.6564195;

        private readonly IDroneService _droneService;

        public PedidoService(IDroneService droneService)
        {
            _droneService = droneService;
        }

        public async Task<DroneDTO> DroneAtendePedido(Pedido pedido)
        {
            double distance = GeoCalculator.GetDistance(LATITUDE_SAIDA_DRONE, LONGITUDE_SAIDA_DRONE, pedido.Latitude, pedido.Longitude, 1,DistanceUnit.Kilometers) * 2;

            var drones = await _droneService.GetAll();
            
            var buscaDrone = drones.Where(d => d.PerfomanceRestante >= distance && d.CapacidadeRestante >= pedido.Peso).FirstOrDefault();

            if (buscaDrone == null)
                return null;

            buscaDrone.PerfomanceRestante -= distance;
            buscaDrone.CapacidadeRestante -= pedido.Peso;

            return new DroneDTO(buscaDrone, distance);
        }
    }
}
