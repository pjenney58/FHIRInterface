using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Administration.Model;

namespace Administration.Controllers
{
    // For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
     //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollectorsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public CollectorsController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ActionResult> GetControllers()
        {
            return BadRequest();
        }
    }
}

