using System.Diagnostics;
using Authentication.Data;
using PalisaidMeta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Support.Model;
using Microsoft.EntityFrameworkCore;

namespace Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly PalisaidMetaContext _context;
        internal readonly ILogger<Patient> _logger;

        public PatientsController(PalisaidMetaContext context, ILogger<Patient> logger)
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

            if (_context != null && _context.Patients != null)
            {
                List<Patient> list;

                try
                {
                    var tid = JwtTenantId.Get(Request);

                    if (tid == Guid.Empty && User != null)
                    {
                        if (User.Identity != null)
                        {
                            var user = User.Identity as System.Security.Claims.ClaimsIdentity;
                            // user.Claims.ToList().ForEach(c => Debug.WriteLine($"Claim: {c.Type} = {c.Value}"));

                            if (user.HasClaim(user.RoleClaimType, "PalisaidRootAdministrator") ||
                               user.HasClaim(user.RoleClaimType, "PalisaidTenantAdministrator"))
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
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return Problem("");
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string patientid)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (_context != null && _context.Patients != null)
            {
                try
                {
                    var tid = JwtTenantId.Get(Request);
                    if (tid == Guid.Empty)
                    {
                        return Ok(await Task.Run(() => _context.Patients.Where(i => i.PrimaryPatientIdString == patientid)));
                    }

                    return Ok(await Task.Run(() => _context.Patients.Where(i => i.PrimaryPatientIdString == patientid && i.TenantId == tid)));
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
        public async Task<IActionResult> GetNotes(string patientid)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpGet("allergies/{patientid}")]
        public async Task<IActionResult> GetAllergies(string patientid)
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
        public async Task<IActionResult> GetEncounters(string patientid)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            return BadRequest("Not Implemented");
        }

        [HttpPost]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Post(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);
            if (patient.EntityId != Guid.Empty && !patient.IsActive && !patient.IsDeleted)
            {
                var p = await _context.Patients.FirstOrDefaultAsync(p => p.EntityId == patient.EntityId);
                if (p != null)
                {
                    _context.Update<Patient>(patient);
                    p.MarkAsUpdated();
                    return Ok(p);
                }
                else
                {
                    await _context.AddAsync<Patient>(patient);
                }

                await _context.SaveChangesAsync();
                return Ok(patient);
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            return BadRequest("Not Implemented");
        }

        [HttpDelete]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
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