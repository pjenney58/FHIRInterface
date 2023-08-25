using System.ComponentModel.DataAnnotations;

namespace ControlPod.Models.Server
{
    public class AuthenticationRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}