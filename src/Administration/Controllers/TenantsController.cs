using System;
using DataShapes.Model;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : Controller
    {
		internal readonly DataShapeContext? _context;

		public TenantsController(DataShapeContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
		{
			try
			{
				if(_context == null)
				{
					return Problem("null context");
				}

				return Ok(_context?.Tenants?.ToList());
			}
			catch(Exception ex)
			{
				return Problem(ex.Message);
			}
		}

        [HttpGet("{id}")]
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

        [HttpPost]
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

