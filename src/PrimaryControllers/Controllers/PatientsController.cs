using Authentication.Data;
using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Support.Model;

namespace Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly DataShapeContext _context;
        internal readonly ILogger<Patient> _logger;

        public PatientsController(DataShapeContext context, ILogger<Patient> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetAll()
        {
            if (_context != null && _context.Patients != null)
            {
                List<Patient> list;

                try
                {
                    var tid = JwtTenantId.Get(Request);

                    //if(User.Claims.Where(p => p.))

                    if (tid == Guid.Empty && User != null)
                    {
                        list = await Task.Run(() => _context.Patients.ToList());
                        return Ok(list.Select(l => new { l?.EntityId, l?.Name?.FamilyName, l?.Name?.FirstName }));
                    }
                    else
                    {
                        list = await Task.Run(() => _context.Patients.Where(t => t.TenantId == tid).ToList());
                        return Ok(list.Select(l => new { l?.EntityId, l?.Name?.FamilyName, l?.Name?.FirstName }));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return Problem("");
        }

        [HttpGet("{patientid}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetById(string id)
        {
            if (_context != null && _context.Patients != null)
            {
                try
                {
                    var tid = JwtTenantId.Get(Request);
                    if (tid == Guid.Empty)
                    {
                        return Ok(await Task.Run(() => _context.Patients.Where(i => i.PrimaryPatientIdString == id)));
                    }

                    return Ok(await Task.Run(() => _context.Patients.Where(i => i.PrimaryPatientIdString == id && i.TenantId == tid)));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return Problem("");
        }

        [HttpGet("{patientname}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetByName(string name)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("notes/{patientid}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetNotes(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("allergies/{patientid}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetAllergies(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("obeservations/{patientid}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetObservations(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("encounters/{patientid}")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> GetEncounters(string id)
        {
            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpPost]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> Post()
        {
            return BadRequest("Not Implemented");
        }

        [HttpPut]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put()
        {
            return BadRequest("Not Implemented");
        }

        [HttpDelete]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Delete()
        {
            return BadRequest("Not Implemented");
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}