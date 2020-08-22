using grupo4.devboost.dronedelivery.Data;
using grupo4.devboost.dronedelivery.Models;
using grupo4.devboost.dronedelivery.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grupo4.devboost.dronedelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DronesController : ControllerBase
    {
        private readonly grupo4devboostdronedeliveryContext _context;
        private readonly IDroneService _droneService;

        public DronesController(grupo4devboostdronedeliveryContext context, IDroneService droneService)
        {
            _context = context;
            _droneService = droneService;
        }

        // GET: api/Drones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrone()
        {
            return await _context.Drone.ToListAsync();
        }

        // GET: api/Drones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drone>> GetDrone(int id)
        {
            var drone = await _context.Drone.FindAsync(id);

            if (drone == null)
            {
                return NotFound();
            }

            return drone;
        }

        // GET: api/Drones/5
        [HttpGet("GetStatusDrone")]
        public async Task<ActionResult<List<DronesPedidosDTO>>> GetStatusDrone()
        {
            return Ok(await _droneService.GetStatusDrone());
        }

        // PUT: api/Drones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrone(int id, Drone drone)
        {
            if (id != drone.Id)
            {
                return BadRequest();
            }

            _context.Entry(drone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DroneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Drones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            drone.Perfomance = (float)(drone.Autonomia / 60.0f) * drone.Velocidade;
            drone.PerfomanceRestante = drone.Perfomance;
            drone.CapacidadeRestante = drone.Capacidade;

            _context.Drone.Add(drone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrone", new { id = drone.Id }, drone);
        }

        // DELETE: api/Drones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drone>> DeleteDrone(int id)
        {
            var drone = await _context.Drone.FindAsync(id);
            if (drone == null)
            {
                return NotFound();
            }

            _context.Drone.Remove(drone);
            await _context.SaveChangesAsync();

            return drone;
        }

        private bool DroneExists(int id)
        {
            return _context.Drone.Any(e => e.Id == id);
        }
    }
}
