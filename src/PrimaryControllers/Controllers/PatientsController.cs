
using PalisaidMeta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using NLog.LayoutRenderers;

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

        [HttpGet("GetAll")]
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
                            if (user == null)
                            {
                                return BadRequest();
                            }

                            if (user.HasClaim(user.RoleClaimType, "PalisaidRootAdministrator") ||
                                user.HasClaim(user.RoleClaimType, "PalisaidTenantAdministrator"))
                            {
                                list = await Task.Run(() => _context.Patients.Include(n => n.Name).ToList());
                                return Ok(list.Select(l => new { l?.EntityId, l?.Name?.FamilyName, l?.Name?.FirstName }));
                            }
                        }

                        return BadRequest();
                    }
                    else
                    {
                        list = await Task.Run(() => _context.Patients.Where(t => t.TenantId == tid).Include(n => n.Name).ToList());
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
        public async Task<IActionResult> GetById(Guid patientid)
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
                        var user = User.Identity as System.Security.Claims.ClaimsIdentity;
                        if (user.HasClaim(user.RoleClaimType, "PalisaidRootAdministrator") ||
                            user.HasClaim(user.RoleClaimType, "PalisaidTenantAdministrator"))
                        {
                            var response = await Task.Run(() => _context.Patients.Where(p => p.EntityId == patientid &&
                                                                                        p.IsActive == true && 
                                                                                        p.IsDeleted == false)
                                                                            .Include(n => n.Name)
                                                                            .Include(a => a.Addresses)
                                                                            .Include(a => a.HL7Identifiers)
                                                                            .Include(l => l.Locations)
                                                                            .Include(cm => cm.ContactMethods)
                                                                            .Include(t => t.Treatments).Include(l => l.Languages)
                                                                            .FirstOrDefault());
                            return Ok(response);
                        }

                        return BadRequest();
                    }
                    else
                    {
                        return Ok(await Task.Run(() => _context.Patients.Where(i => i.EntityId == patientid && 
                                                                               i.TenantId == tid && 
                                                                               i.IsActive == true && 
                                                                               i.IsDeleted == false)
                                                                        .Include(n => n.Name)
                                                                        .Include(a => a.Addresses)
                                                                        .Include(a => a.HL7Identifiers)
                                                                        .Include(l => l.Locations)
                                                                        .Include(cm => cm.ContactMethods)
                                                                        .Include(l => l.Languages)
                                                                        .FirstOrDefault()));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return BadRequest();
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string? prefix, string? givenname, string? middlename, string familyname, string? suffix)
        {
            var predicate = PredicateBuilder.New<Patient>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.

            if (!string.IsNullOrEmpty(prefix))
                predicate = predicate.And(p => p.Name.Prefix.Contains(prefix));

            if (!string.IsNullOrEmpty(givenname))
                predicate = predicate.And(p => p.Name.GivenName.Contains(givenname));

            if (!string.IsNullOrEmpty(middlename))
                predicate = predicate.And(p => p.Name.MiddleName == middlename);

            predicate = predicate.And(p => p.Name.FamilyName == familyname);

            if (!string.IsNullOrEmpty(suffix))
                predicate = predicate.And(p => p.Name.Suffix.Contains(suffix));

            predicate = predicate.And(p => p.IsActive == true && p.IsDeleted == false);

#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (_context != null && _context.Patients != null)
            {
                try
                {
                    var tid = JwtTenantId.Get(Request);

                    if (tid == Guid.Empty && User != null)
                    {
                        if (User.Identity != null)
                        {
                            var user = User.Identity as System.Security.Claims.ClaimsIdentity;
                            if (user == null)
                            {
                                return BadRequest();
                            }

                            if (user.HasClaim(user.RoleClaimType, "PalisaidRootAdministrator") ||
                                user.HasClaim(user.RoleClaimType, "PalisaidTenantAdministrator"))
                            {
                                return Ok(await Task.Run(() => _context.Patients.Where(predicate)
                                                                                .Include(n => n.Name)
                                                                                .Include(a => a.Addresses)
                                                                                .Include(a => a.HL7Identifiers)
                                                                                .Include(l => l.Locations)
                                                                                .Include(cm => cm.ContactMethods)
                                                                                .Include(l => l.Languages)));
                            }
                        }

                        return BadRequest();
                    }
                    else
                    {
                        predicate = predicate.And(t => t.TenantId == tid);
                        return Ok(await Task.Run(() => _context.Patients.Where(predicate)
                                                                        .Include(n => n.Name)
                                                                        .Include(a => a.Addresses)
                                                                        .Include(a => a.HL7Identifiers)
                                                                        .Include(l => l.Locations)
                                                                        .Include(cm => cm.ContactMethods)
                                                                        .Include(l => l.Languages)));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Problem(ex.Message);
                }
            }

            return BadRequest();
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

        [HttpPost("AddNewPatient")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> Post(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var tid = JwtTenantId.Get(Request);

            try
            {
                patient.TenantId = patient.TenantId == Guid.Empty 
                        ? patient.DefaultTenantId 
                        : tid;

                patient.OriginId = patient.EntityId.ToString();

                patient.MarkAsUpdated();
                await _context.AddAsync<Patient>(patient);
                await _context.SaveChangesAsync();
            
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("UpdatePatient")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            patient.PrimaryPatientIdString = patient.EntityId.ToString();

            try
            {
                patient.MarkAsUpdated();
                _context.Update<Patient>(patient);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("DeletePatient")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var patient = await _context.Patients.FindAsync(Guid.Parse(Request.Query["patientid"]));
                patient.MarkDeleted();
                _context.Update<Patient>(patient);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest();
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}