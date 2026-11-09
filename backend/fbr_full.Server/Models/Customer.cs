namespace fbr_full.Server.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? NTN { get; set; } // National Tax Number
        public string? RegistrationNumber { get; set; }
        public string? TaxId { get; set; }
        public string? Scenario { get; set; } // For FBR integration scenarios
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
} 