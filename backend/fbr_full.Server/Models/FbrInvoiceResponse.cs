namespace fbr_full.Server.Models
{
    public class FbrInvoiceResponse
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string FbrReferenceNumber { get; set; } = string.Empty;
        public string? ResponseData { get; set; }
        public string Status { get; set; } = string.Empty; // Success, Failed, Pending
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Invoice Invoice { get; set; } = null!;
    }
} 