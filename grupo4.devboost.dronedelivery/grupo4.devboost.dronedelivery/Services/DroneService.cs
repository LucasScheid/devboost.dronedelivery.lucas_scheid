using Dapper;
using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public class DroneService : IDroneService
    {
        private readonly grupo4devboostdronedeliveryContext _context;

        public DroneService(grupo4devboostdronedeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drone>> GetAll()
        {
            return await _context.Drone.ToListAsync();
        }

        public async Task<List<DronesPedidosDTO>> GetStatusDrone()
        {
            //Foi descontado 3 horas da data atual pois o fuso do banco de dados estava errado
            string sqlCommand = @"select a.DroneId,
                                         0 as Situacao,
                                         a.Id as PedidoId 
                                  from pedido a
                                  where a.Situacao <> 2
                                  and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)
                                  union
                                  select b.Id as DroneId,
                                         1 as Situacao,
                                         0 as PedidoId
                                  from  Drone b
                                  where b.Id NOT IN  (
                                      select a.DroneId     
                                  from pedido a
                                  where a.Situacao <> 2
                                  and a.DataHoraFinalizacao > dateadd(hour,-3,CURRENT_TIMESTAMP)
                                  ) ";

            using SqlConnection conexao = new SqlConnection("server=localhost;database=desafio-drone-db;user id=sa;password=minha@password");

            var drones = await conexao.QueryAsync<StatusDroneDTO>(sqlCommand);

            List<DronesPedidosDTO> lDronesPedidosDTO = new List<DronesPedidosDTO>();

            foreach (var drone in drones)
            {
                DronesPedidosDTO dronesPedidosDTO = new DronesPedidosDTO
                {
                    DroneId = drone.DroneId,
                    Situacao = drone.Situacao ? "Disponivel" : "Ocupado"
                };

                var existeDrone = lDronesPedidosDTO.Where(d => d.DroneId == drone.DroneId).FirstOrDefault();

                if (existeDrone != null)
                {
                    lDronesPedidosDTO.Remove(existeDrone);
                    existeDrone.Pedidos.Add(drone.PedidoId);
                    lDronesPedidosDTO.Add(existeDrone);
                }
                else
                {
                    dronesPedidosDTO.Pedidos.Add(drone.PedidoId);
                    lDronesPedidosDTO.Add(dronesPedidosDTO);
                }
            }

            await LiberarDrones(lDronesPedidosDTO);

            return lDronesPedidosDTO;
        }
        private async Task LiberarDrones(List<DronesPedidosDTO> statusDrone)
        {            
            foreach (var drone in statusDrone)
            {
                if (drone.Situacao == "Disponivel")
                {
                    var droneBase = await _context.Drone.FindAsync(drone.DroneId);

                    if (droneBase != null)
                    {
                        if (droneBase.Perfomance != droneBase.PerfomanceRestante)
                        {
                            droneBase.PerfomanceRestante = droneBase.Perfomance;
                            droneBase.CapacidadeRestante = droneBase.Capacidade;
                            _context.Entry(droneBase).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}
