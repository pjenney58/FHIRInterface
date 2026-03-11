using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PalisaidMeta.Model;
using Support.Model;

namespace Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PractitionersController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly PalisaidMetaContext? _context;
        internal readonly ILogger<Practitioner> _logger;

        public PractitionersController(PalisaidMetaContext context, ILogger<Practitioner> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
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
                return BadRequest("Not Implemented");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Post()
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