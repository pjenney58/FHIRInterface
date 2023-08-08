using Microsoft.AspNetCore.Identity;

namespace Authentication.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public Guid TenantId { get; set; }
    }
}
