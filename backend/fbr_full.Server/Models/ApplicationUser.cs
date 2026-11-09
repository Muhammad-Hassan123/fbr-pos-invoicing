using Microsoft.AspNetCore.Identity;

namespace fbr_full.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? StoreName { get; set; }
        public string? StoreAddress { get; set; }
        public string? NTN { get; set; }
        public string? STRN { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
} 