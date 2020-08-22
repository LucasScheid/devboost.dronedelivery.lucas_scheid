using grupo4.devboost.dronedelivery.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public interface IPedidoService
    {
        Task<DroneDTO> DroneAtendePedido(Pedido pedido);
        DateTime BuscarDataEntregaPedidoAbertoDrone(IEnumerable<Pedido> pedidos, int droneId);
    }
}
