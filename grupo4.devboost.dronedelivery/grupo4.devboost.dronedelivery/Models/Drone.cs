using System.ComponentModel.DataAnnotations;

namespace grupo4.devboost.dronedelivery.Models
{
    public class Drone
    {
        [Key]
        public int Id { get; set; }
        public int Capacidade { get; set; }
        public int CapacidadeRestante { get; set; }
        public int Velocidade { get; set; }
        public int Autonomia { get; set; }
        public int Carga { get; set; }
        public double Perfomance { get; set; }
        public double PerfomanceRestante { get; set; }
    }
}
