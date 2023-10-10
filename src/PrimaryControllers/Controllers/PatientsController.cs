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
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if(_context != null && _context.Patients != null)
            {
                List<Patient> list;

                try
                {
                    var tid = JwtTenantId.Get(Request);
                
                    if(tid == Guid.Empty && User != null)
                    {
                        if (User.Identity != null)
                        {
                            if (((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim("role", "PalisaidRootAdministrator") ||
                               ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim("role", "PalisaidTenantAdministrator"))
                            {
                                list = await Task.Run(() => _context.Patients.ToList());
                                return Ok(list.Select(l => new { l?.EntityId, l?.Name?.FamilyName, l?.Name?.FirstName }));
                            }
                        }

                        return BadRequest();
                    }
                    else
                    {
                            list = await Task.Run(() => _context.Patients.Where(t => t.TenantId == tid).ToList());         
                            return Ok(list.Select(l => new { l?.EntityId, l?.Name?.FamilyName, l?.Name?.FirstName }));
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return Problem("");
        }

        [HttpGet("{patientid}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if(_context != null && _context.Patients != null)
            {
                try
                {              
                    var tid = JwtTenantId.Get(Request);
                    if(tid == Guid.Empty)
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
        public async Task<IActionResult> GetByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("notes/{patientid}")]
        public async Task<IActionResult> GetNotes(string id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("allergies/{patientid}")]
        public async Task<IActionResult> GetAllergies(string id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("obeservations/{patientid}")]
        public async Task<IActionResult> GetObservations(string id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("encounters/{patientid}")]
        public async Task<IActionResult> GetEncounters(string id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpPost]
        [Authorize(Roles="PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Post()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            return BadRequest("Not Implemented");
        }

        [HttpPut]
        [Authorize(Roles="PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            return BadRequest("Not Implemented");
        }

        [HttpDelete]
        [Authorize(Roles="PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            
            return BadRequest("Not Implemented");
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}