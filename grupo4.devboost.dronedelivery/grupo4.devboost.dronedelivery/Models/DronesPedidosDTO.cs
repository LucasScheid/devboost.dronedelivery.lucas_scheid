using System;
using System.Collections.Generic;

namespace grupo4.devboost.dronedelivery.Models
{
    public class DronesPedidosDTO
    {
        public int DroneId { get; set; }
        public string Situacao { get; set; }
        public List<int> Pedidos { get; set; }

        public DateTime DataHoraEstaraDisponivel { get; set; }

        public DronesPedidosDTO()
        {
            Pedidos = new List<int>();
        }

    }
}
