using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fbr_full.Server.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? StoreName { get; set; }
        public string? StoreAddress { get; set; }
        public string? NTN { get; set; }
        public string? STRN { get; set; }
    }

    public class AuthResponse
    {
        [JsonPropertyName("Token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }
        [JsonPropertyName("User")]
        public UserProfile User { get; set; } = new();
    }

    public class UserProfile
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("StoreName")]
        public string? StoreName { get; set; }
        [JsonPropertyName("StoreAddress")]
        public string? StoreAddress { get; set; }
        [JsonPropertyName("NTN")]
        public string? NTN { get; set; }
        [JsonPropertyName("STRN")]
        public string? STRN { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
} 