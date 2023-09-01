using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Administration.Controllers
{
    [Authorize]
    [Route("admin/[controller]")]
    [ApiController]
    public class SystemsController : Controller
    {
        internal readonly DataShapeContext? _context;

        public SystemsController(DataShapeContext context)
        {
            _context = context;
        }

        [HttpGet]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Get()

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