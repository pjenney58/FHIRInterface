/*
 * Software License - AuthenticateController.cs
 *
 *  Copyright (c) 2022-Present by Palisaid, LLC
 *
 * Permission is hereby granted, in consideration of a license fee or agreement,
   * to any person obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute,
 * and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 *The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
 * JWT Implementation drawn from --
 *      https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-web-api-with-json-web-tokens/
 *
 *  It's not too bad ...
 */

using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DataShapes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Support.Model;

namespace Primary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthenticateController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (login == null ||
                string.IsNullOrEmpty(login.Password) ||
                string.IsNullOrEmpty(login.Username))
            {
                return BadRequest("Null parameter");
            }

            try
            {
                var user = await _userManager.Users.FirstAsync(u => u.UserName == login.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.Username),
                        new Claim(ClaimTypes.PrimarySid, user.TenantId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = CreateToken(authClaims);
                    var refreshToken = GenerateRefreshToken();

                    int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        Debug.WriteLine($"Failed while updating user");
                        foreach (var error in result.Errors)
                        {
                            Debug.WriteLine(error.Description);
                        }
                    }

                    _logger.LogInformation($"User {user.Id} logged in.");

                    return Ok(new
                    {
                        accesstoken = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshtoken = refreshToken,
                        validTo = token.ValidTo
                    });
                }
            }
            catch (InvalidDataException)
            { }

            _logger.LogInformation($"Login failure {login.Username}.");

            return Unauthorized();
        }

        [HttpGet]
        [Route("users")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            DisposableList<ApplicationUser> users = new();

            var result = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "PalisaidUser"));
            users.AddRange(result);

            result = await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "TenantUser"));
            users.AddRange(result);

            return Ok(users);
        }

        [HttpPost]
        [Route("register-user")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel? model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (model == null ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Username))
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
                PhoneNumber = model.Phone,
                TwoFactorEnabled = model.Use2Factor,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantId = model.Tenant
            };

            if (await _roleManager.RoleExistsAsync("TenantUser"))
            {
                await _userManager.AddToRoleAsync(user, "TenantUser");
            }

            if (await _roleManager.RoleExistsAsync("Everyone"))
            {
                await _userManager.AddToRoleAsync(user, "Everyone");
            }

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

            return Ok(StatusCodes.Status201Created);
        }

        // Register Palisaid/Principal Users
        [HttpPost]
        [Route("principal-user")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidOwner")]
        public async Task<IActionResult> RegistePrincipal([FromBody] RegisterModel? model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (model == null ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Username))
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
                PhoneNumber = model.Phone,
                TwoFactorEnabled = model.Use2Factor,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                TenantId = JwtTenantId.Get(Request)
            };

            if (await _roleManager.RoleExistsAsync("PalisaidUser"))
            {
                await _userManager.AddToRoleAsync(user, "PalisaidUser");
            }

            if (await _roleManager.RoleExistsAsync("Everyone"))
            {
                await _userManager.AddToRoleAsync(user, "Everyone");
            }

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

            return Ok(new Response { Status = "Success", Message = $"User {model.Username} created!" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register-principal-admin")]
        [Authorize(Roles = "PalisaidOwner")]
        public async Task<IActionResult> RegisterPrincipalAdmin([FromBody] RegisterAdminModel? model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (model == null ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Username))
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
                TenantId = Guid.Empty
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

            if (!await _roleManager.RoleExistsAsync("PalisaidRootAdministrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole("PalisaidRootAdministrator"));
            }

            if (!await _roleManager.RoleExistsAsync("PalisaidTenantAdministrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole("PalisaidTenantAdministrator"));
            }

            if (!await _roleManager.RoleExistsAsync("PalisaidUser"))
            {
                await _roleManager.CreateAsync(new IdentityRole("PalisaidUser"));
            }

            if (await _roleManager.RoleExistsAsync("PalisaidRootAdministrator"))
            {
                await _userManager.AddToRoleAsync(user, "PalisaidRootAdministrator");
            }

            if (await _roleManager.RoleExistsAsync("PalisaidRootAdministrator"))
            {
                await _userManager.AddToRoleAsync(user, "PalisaidRootAdministrator");
            }

            if (await _roleManager.RoleExistsAsync("PalisaidTenantAdministrator"))
            {
                await _userManager.AddToRoleAsync(user, "PalisaidTenantAdministrator");
            }

            if (await _roleManager.RoleExistsAsync("PalisaidUser"))
            {
                await _userManager.AddToRoleAsync(user, "PalisaidUser");
            }

            if (await _roleManager.RoleExistsAsync("Everyone"))
            {
                await _userManager.AddToRoleAsync(user, "Everyone");
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register-admin")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidOwner")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminModel? model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (model == null ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Username))
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
                TenantId = Guid.Empty
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

            if (!await _roleManager.RoleExistsAsync("TenantRootAdministrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole("TenantRootAdministrator"));
            }

            if (!await _roleManager.RoleExistsAsync("TenantUser"))
            {
                await _roleManager.CreateAsync(new IdentityRole("TenantUser"));
            }

            if (await _roleManager.RoleExistsAsync("TenantRootAdministrator"))
            {
                await _userManager.AddToRoleAsync(user, "TenantRootAdministrator");
            }

            if (await _roleManager.RoleExistsAsync("TenantUser"))
            {
                await _userManager.AddToRoleAsync(user, "TenantUser");
            }

            if (await _roleManager.RoleExistsAsync("Everyone"))
            {
                await _userManager.AddToRoleAsync(user, "Everyone");
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        #region RoleManagement

        [HttpGet]
        [Route("role")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public IActionResult GetRoles()
        {
            if (ModelState.IsValid)
            {
                return Ok(_roleManager.Roles);
            }

            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        [HttpPost]
        [Route("role/{role}")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> AddRole(string role)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(role));
            return Ok();
        }

        [HttpDelete]
        [Route("role/{role}")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> DeleteRole(string role)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            IdentityResult result = await _roleManager.DeleteAsync(new IdentityRole(role));
            return Ok();
        }

        [HttpPut]
        [Route("role/{id,role}")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> AddUserRole(Guid id, string role)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var _user = await _userManager.FindByIdAsync(id.ToString());
                if (_user != null)
                {
                    await _userManager.AddToRoleAsync(_user, role);
                    return Ok();
                }

                _logger.LogInformation("null _user object return");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("role/{id,role}")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> DeleteUserRole(Guid id, string role)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var _user = await _userManager.FindByIdAsync(id.ToString());
                if (_user != null)
                {
                    await _userManager.RemoveFromRoleAsync(_user, role);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        [HttpGet]
        [Route("role/{id}")]
        [Authorize(Roles = "PalisaidRootAdministrator, PalisaidTenantAdministrator, PalisaidOwner")]
        public async Task<IActionResult> GetUserRoles(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var _user = await _userManager.FindByIdAsync(id.ToString());
                if (_user != null)
                {
                    return Ok(await _userManager.GetRolesAsync(_user));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        #endregion RoleManagement

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null || tokenModel.AccessToken is null || tokenModel.RefreshToken is null)
            {
                return BadRequest("tokenmodel");
            }

            try
            {
                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return BadRequest("Invalid access token or refresh token");
                }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                var user = await _userManager.FindByNameAsync(username ?? throw new ArgumentNullException("principal.Identity.Name"));

                if (user == null || user.RefreshToken != refreshToken)
                {
                    return BadRequest("Invalid access token or refresh token");
                }

                var token = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = token.ValidFrom;

                Debug.WriteLine($"New refresh token is: {newRefreshToken}");

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    Debug.WriteLine($"Failed while updating user");
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine(error.Description);
                    }
                }

                return Ok(new
                {
                    accesstoken = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshtoken = newRefreshToken,
                    validTo = token.ValidTo
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("revoke/{username}")]
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

        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = await _userManager.Users.Where(t => t.TenantId == JwtTenantId.Get(Request)).ToListAsync();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validata")]
        public async Task<ActionResult<bool>> ValidateUser(string username, string token)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Invalid user name");
            }

            return false;
        }

        internal JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret")));
            if (!double.TryParse(_configuration["JWT:TokenValidityInSeconds"], out double tokenValidityInSeconds))
            {
                tokenValidityInSeconds = 10;
            }

            var validTo = DateTime.Now.ToLocalTime().AddSeconds(tokenValidityInSeconds);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: validTo,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        internal static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        internal ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret"))),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}