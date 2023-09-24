using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;

namespace Primary.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PractitionersController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly DataShapeContext? _context;
        internal readonly ILogger<Practitioner> _logger;

        public PractitionersController(DataShapeContext context, ILogger<Practitioner> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tid = JwtTenantId.Get(Request);
            var docs = _context.Practitioners.ToList();

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