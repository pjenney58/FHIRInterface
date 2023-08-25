using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Support.Model;

namespace Administration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Register([FromBody] RegisterModel? model)
        {
            if (model == null)
            {
                return BadRequest("Null parameter");
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantId = JwtTenantId.Get(Request)
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Code}\n{error.Description}\n";
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = errors });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterModel? model)
        {
            if (model == null)
            {
                return BadRequest("Null parameter");
            }

            var userExists = await _userManager.Users.FirstAsync(u => u.UserName == model.Username && u.TenantId == JwtTenantId.Get(Request));
            //var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantId = JwtTenantId.Get(Request)
            };

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Code}\n{error.Description}\n";
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = errors });
            }

            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }

        [HttpPost]
        [Route("logout/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.Users.FirstAsync(u => u.UserName == username && u.TenantId == JwtTenantId.Get(Request));
            //var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Invalid user name");
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}