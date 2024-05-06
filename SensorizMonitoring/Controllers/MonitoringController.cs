using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SensorizMonitoring.Business;
using SensorizMonitoring.Data.Context;
using SensorizMonitoring.Data.Models;
using SensorizMonitoring.Models;

namespace SensorizMonitoring.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MonitoringController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public MonitoringController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public IActionResult InsertMonitoring([FromBody] MonitoringModel mnt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var insertMonitoring = new Monitoring();

                insertMonitoring.device_id = mnt.deviceId;
                insertMonitoring.temperature = mnt.status.temperature;
                insertMonitoring.atmospheric_pressure = mnt.status.atmosphericPressure;
                insertMonitoring.lat = mnt.pos.lat;
                insertMonitoring.lon = mnt.pos.lon;
                insertMonitoring.cep = mnt.pos.cep;
                insertMonitoring.external_power = mnt.status.externalPower;
                insertMonitoring.charging = mnt.status.charging;
                insertMonitoring.battery_voltage = mnt.status.batteryVoltage;
                insertMonitoring.light_level = mnt.status.lightLevel;
                insertMonitoring.orientation_x = mnt.status.orientation.x;
                insertMonitoring.orientation_y = mnt.status.orientation.y;
                insertMonitoring.orientation_z = mnt.status.orientation.z;
                insertMonitoring.vibration_x = mnt.status.vibration.x;
                insertMonitoring.vibration_y = mnt.status.vibration.y;
                insertMonitoring.vibration_z = mnt.status.vibration.z;
                insertMonitoring.com_signal = mnt.status.signal;
                insertMonitoring.tamper = mnt.status.tamper;
                insertMonitoring.movement = mnt.status.movement;
                insertMonitoring.created_at = DateTime.Now;

                _context.Add(insertMonitoring);
                _context.SaveChanges();
                return Ok(insertMonitoring);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, "Error insert monitoring: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Monitoring>>> GetMonitoring()
        {
            var monitorings = await _context.Monitoring.AsNoTracking().ToListAsync();
            return Ok(monitorings);
        }
    }
}