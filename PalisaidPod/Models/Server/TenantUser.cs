namespace PalisaidPod.Models.Server
{
    using Microsoft.AspNetCore.Identity;

    public class TenantUser : IdentityUser
    {
        public Guid TenantId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}