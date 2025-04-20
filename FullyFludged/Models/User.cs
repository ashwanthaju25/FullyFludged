using System.ComponentModel.DataAnnotations;

namespace FullyFludged.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public byte[]? PasswordSalt { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Role { get; set; } = "User";

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // 🔐 Refresh Token fields
        [MaxLength(500)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
