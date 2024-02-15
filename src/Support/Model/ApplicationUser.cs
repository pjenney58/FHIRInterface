//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace Support.Model
{
    public class ApplicationUser : IdentityUser, IDisposable
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public Guid TenantId { get; set; }

        /// <summary>
        /// The entity id of the user, useful if the user is a practitioner or a patient
        /// </summary>
        public string? EntityId { get; set; }

        public void Dispose()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = DateTime.MinValue;
            TenantId = Guid.Empty;
        }
    }
}