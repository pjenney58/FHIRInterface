using System.ComponentModel.DataAnnotations;

namespace PalisaidPod.Models.Server
{
    public class AuthenticationRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}