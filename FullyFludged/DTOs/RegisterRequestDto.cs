using System.ComponentModel.DataAnnotations;

namespace FullyFludged.DTOs
{
    public class RegisterRequestDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
