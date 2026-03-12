using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PalisaidMeta.Model;

namespace Administration.Controllers
{
    [Authorize]
    [Route("admin/[controller]")]
    [ApiController]
    public class TenantsController : Controller
    {
        internal readonly PalisaidMetaContext? _context;
        internal readonly Guid Root = new Guid("{10000000-0000-0000-0000-000000000000}");

        public TenantsController(PalisaidMetaContext context)
        {
            _context = context;
            if (_context.Tenants is null)
            {
                throw new ArgumentNullException("Tenants");
            }
        }

        [HttpGet]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                if (_context == null)
                {
                    return Problem("null context");
                }

                return Ok(_context?.Tenants?.ToList());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.

        [HttpGet("{id}")]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid id)
        {
            try
            {
                if (_context == null)
                {
                    return Problem("null context");
                }

                return Ok(await _context.Tenants.FindAsync(id));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{name}")]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<Tenant>> CreateNewEmptyTenant(string name)
        {
            if (_context == null)
            {
                return Problem("null context");
            }

            try
            {
                var newTenant = new Tenant()
                {
                    Name = name ?? "NoName",
                    TenantId = Guid.NewGuid(),
                    EntityId = Guid.NewGuid().ToString(),
                    OwnerId = Root
                };

                await _context.Tenants.AddAsync(newTenant);
                await _context.SaveChangesAsync();

                return Ok(newTenant);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<Tenant>> AddTenant(Tenant tenant)
        {
            try
            {
                if (_context == null)
                {
                    return Problem("null context");
                }

                await _context.Tenants.AddAsync(tenant);
                await _context.SaveChangesAsync();

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<Tenant>> UpdateTenant(Tenant tenant)
        {
            try
            {
                if (_context == null)
                {
                    return Problem("null context");
                }

                _context.Tenants.Update(tenant);
                await _context.SaveChangesAsync();

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Policy = "PalisaidTenantAdministrator")]
        public async Task<ActionResult<Tenant>> DeleteTenant(Tenant tenant)
        {
            try
            {
                if (_context == null)
                {
                    return Problem("null context");
                }

                _context.Tenants.Remove(tenant);
                await _context.SaveChangesAsync();

                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}