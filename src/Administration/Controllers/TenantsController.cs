using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    [Authorize]
    [Route("admin/[controller]")]
    [ApiController]
    public class TenantsController : Controller
    {
        internal readonly DataShapeContext? _context;
        internal readonly Guid Root = new Guid("{10000000-0000-0000-0000-000000000000}");

        public TenantsController(DataShapeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-tenants")]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
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

        [HttpGet("{id}")]
        [Route("get-tenant-by-id")]
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
        [Route("create-new-tenant")]
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
                    EntityId = Guid.NewGuid(),
                    OwnerId = Root
                };

                await _context.Tenants.AddAsync(newTenant);
                await _context.SaveChangesAsync();

                return Ok(newTenant);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("add-existing-tenant")]
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
        [Route("update-tenant")]
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
        [Route("delete-tenant")]
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
    }
}

