using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Model
{
    public class User : IdentityUser, IDisposable, IEquatable<User?>
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

        public override bool Equals(object? obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User? other)
        {
            return other is not null &&
                   TenantId.Equals(other.TenantId) &&
                   Password == other.Password &&
                   Email == other.Email &&
                   JwtToken == other.JwtToken &&
                   EqualityComparer<byte[]?>.Default.Equals(Key, other.Key);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TenantId, Password, Email, JwtToken, Key);
        }

        public static bool operator ==(User? left, User? right)
        {
            return EqualityComparer<User>.Default.Equals(left, right);
        }

        public static bool operator !=(User? left, User? right)
        {
            return !(left == right);
        }
    }
}