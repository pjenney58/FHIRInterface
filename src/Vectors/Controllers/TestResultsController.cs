using PalisaidMeta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lists.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultsController : Controller
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal readonly PalisaidMetaContext? _context;
        internal readonly ILogger<TestResultEntry> _logger;

        public TestResultsController(PalisaidMetaContext context, ILogger<TestResultEntry> logger)
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

