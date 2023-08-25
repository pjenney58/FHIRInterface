using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Support.Model
{
    public class User : IdentityUser, IDisposable
    {
        /// <summary>
        /// The owner of this user record
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? JwtToken { get; set; }

        public byte[]? Key { get; set; }

        public User()
        { }

        public void Dispose()
        {
            Key = null;
            JwtToken = null;
            TenantId = Guid.Empty;
            GC.Collect();
        }
    }
}