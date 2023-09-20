using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;

namespace Administration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly DataShapeContext? _context;
        internal readonly ILogger _logger;

        public PatientsController(DataShapeContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tid = JwtTenantId.Get(Request);
                return Ok(await Task.Run(() => _context.Patients.Where(t => t.TenantId == tid).ToList()));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Problem("");
        }

        [HttpGet("{patientid}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var tid = JwtTenantId.Get(Request);
                return Ok(await Task.Run(() => _context.Patients.Where(i => i.PrimaryPatientIdString == id && i.TenantId == tid)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Problem("");
        }

        [HttpGet("{patientname}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("notes/{patientid}")]
        public async Task<IActionResult> GetNotes(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }


        [HttpGet("allergies/{patientid}")]
        public async Task<IActionResult> GetAllergies(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("obeservations/{patientid}")]
        public async Task<IActionResult> GetObservations(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("encounters/{patientid}")]
        public async Task<IActionResult> GetEncounters(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return BadRequest("Not Implemented");
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            return BadRequest("Not Implemented");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return BadRequest("Not Implemented");
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}