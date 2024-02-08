using PalisaidMeta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using NLog.LayoutRenderers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly PalisaidMetaContext? _context;
        internal readonly ILogger<Location> _logger;

        public LocationsController(PalisaidMetaContext context, ILogger<Location> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Everyone")]
        public async Task<IActionResult> Get()
        {
           if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (_context != null && _context.Locations != null)
            {
                List<Location> list;

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
                                list = await  _context.Locations.ToListAsync();
                                return Ok(list.Select(l => new { l?.EntityId, l?.Name}));
                            }
                        }

                        return BadRequest();
                    }
                    else
                    {
                        list = await  _context.Locations.Where(t => t.TenantId == tid).ToListAsync();
                        return Ok(list.Select(l => new { l?.EntityId, l?.Name }));
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
        public async Task<IActionResult> GetById(Guid locationid)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (_context != null && _context.Locations != null)
            {
                try
                {
                    var tid = JwtTenantId.Get(Request);
                    if (tid == BaseConstants.DefaultTenantId)
                    {
                        var user = User.Identity as System.Security.Claims.ClaimsIdentity;
                        if (user.HasClaim(user.RoleClaimType, "PalisaidRootAdministrator") ||
                            user.HasClaim(user.RoleClaimType, "PalisaidTenantAdministrator"))
                        {
                            var response = await Task.Run(() => _context.Locations.Where(p => p.EntityId == locationid &&
                                                                                        p.IsActive == true && 
                                                                                        p.IsDeleted == false)
                                                                                    .Include(a => a.Addresses)                                                                           
                                                                                    .Include(cm => cm.ContactMethods)
                                                                                    .Include(cn => cn.Contacts)
                                                                                    .FirstOrDefault());
                            return Ok(response);
                        }

                        return BadRequest();
                    }
                    else
                    {
                        return Ok(await Task.Run(() => _context.Locations.Where(i => i.EntityId == locationid && 
                                                                               i.TenantId == tid && 
                                                                               i.IsActive == true && 
                                                                               i.IsDeleted == false)
                                                                            .Include(a => a.Addresses)                                                                           
                                                                            .Include(cm => cm.ContactMethods)
                                                                            .Include(cn => cn.Contacts)
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


        [HttpPost]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Post(Location location)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                await _context.AddAsync(location);
                await _context.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Put(Location location)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
               
                location.MarkDeleted();
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpPost("RecoverLocation")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator")]
        public async Task<IActionResult> Undelete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
               
                location.UnDelete();
                await _context.SaveChangesAsync();
                return Ok(location);
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


