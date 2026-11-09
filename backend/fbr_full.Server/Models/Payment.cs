namespace fbr_full.Server.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Card, Bank Transfer, etc.
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Invoice Invoice { get; set; } = null!;
    }
} 