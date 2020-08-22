using Dapper;
using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Services
{
    public class DroneService : IDroneService
    {
        private readonly grupo4devboostdronedeliveryContext _context;

        private readonly string _sqlCommand = @"select a.DroneId,
                                                       0 as Situacao,
                                                       a.Id as PedidoId 
                                                  from pedido a
                                                 where a.Situacao = 0
                                                   and a.DataHoraFinalizacao > @DataHoraAtual
                                                       union
                                                select b.Id as DroneId,
                                                       1 as Situacao,
                                                       0 as PedidoId
                                                  from Drone b
                                                 where b.Id NOT IN (select a.DroneId     
                                                                      from pedido a
                                                                     where a.Situacao = 0
                                                                       and a.DataHoraFinalizacao > @DataHoraAtual)";

        private readonly string _sqlDataHora = @"select max(DataHoraFinalizacao) as DataHoraFinalizacao
                                                   from pedido
                                                  where DroneId = @DroneId
                                                    and Situacao = 0";

        public DroneService(grupo4devboostdronedeliveryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drone>> GetAll()
        {
            return await _context.Drone.ToListAsync();
        }

        public async Task<List<DronesPedidosDTO>> GetStatusDrone(string connectionString)
        {
            using SqlConnection conexao = new SqlConnection(connectionString);

            var drones = await conexao.QueryAsync<StatusDroneDTO>(_sqlCommand, new { DataHoraAtual = DateTime.Now });

            List<DronesPedidosDTO> lDronesPedidosDTO = new List<DronesPedidosDTO>();

            foreach (var drone in drones)
            {
                DronesPedidosDTO dronesPedidosDTO = new DronesPedidosDTO
                {
                    DroneId = drone.DroneId,
                    Situacao = drone.Situacao ? "Disponivel" : "Atendendo Pedidos"
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
                    var datahoraDisponivel = await conexao.QueryAsync<LiberacaoDroneDTO>(_sqlDataHora, new { drone.DroneId });
                    dronesPedidosDTO.DataHoraEstaraDisponivel = datahoraDisponivel.FirstOrDefault().DataHoraFinalizacao == null ? DateTime.Now : Convert.ToDateTime(datahoraDisponivel.FirstOrDefault().DataHoraFinalizacao);

                    dronesPedidosDTO.Pedidos.Add(drone.PedidoId);
                    lDronesPedidosDTO.Add(dronesPedidosDTO);
                }
            }

            await LiberarDrones(lDronesPedidosDTO);
            await FinalizarPedidos();

            return lDronesPedidosDTO.OrderBy(o=>o.DataHoraEstaraDisponivel).ToList();
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

        private async Task FinalizarPedidos()
        {
            var pedidos = await _context.Pedido.ToListAsync();

            foreach (var pedido in pedidos.Where(p => p.Situacao == (int)EStatusPedido.DRONE_ASSOCIADO && p.DataHoraFinalizacao < DateTime.Now))
            {
                pedido.Situacao = (int)EStatusPedido.FINALIZADO;
                _context.Entry(pedido).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

    }
}
