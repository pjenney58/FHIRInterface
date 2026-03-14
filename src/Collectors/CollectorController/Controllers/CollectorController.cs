using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalisaidMeta.Model;
using Support.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Collector.Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly PalisaidMetaContext? _context;
        internal readonly ILogger<Location> _logger;
        internal CollectorConfig? collectorConfig;

        public CollectorController(PalisaidMetaContext context, ILogger<Location> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetDeployed")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);

            try
            {
                return Ok(await _context.Collector.Where(t => t.TenantId == tid).ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Post(CollectorConfig collectorConfig)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            this.collectorConfig = collectorConfig;

            try
            {
                var collector = CollectorFactory.Create(collectorConfig.DataProtocolIn);
                await collector.RegisterCollector();
                await collector.RegisterTransformer(collectorConfig.DataProtocolIn);
                await collector.RegisterTransporter();
                await collector.RegisterScheduler();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }

            return BadRequest("Error");
        }

        [HttpPut]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                return BadRequest("Not Implemented");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                return BadRequest("Not Implemented");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}