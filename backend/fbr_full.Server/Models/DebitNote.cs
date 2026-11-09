namespace fbr_full.Server.Models
{
    public class DebitNote
    {
        public int Id { get; set; }
        public string DebitNoteNumber { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public DateTime IssueDate { get; set; }
        public string? Note { get; set; }
        public decimal InvoiceDiscount { get; set; } = 0;
        public decimal SubTotal { get; set; } = 0;
        public decimal TotalTax { get; set; } = 0;
        public decimal TotalDiscount { get; set; } = 0;
        public decimal GrandTotal { get; set; } = 0;
        public string Status { get; set; } = "Draft"; // Draft, Pending, Completed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Invoice Invoice { get; set; } = null!;
        public virtual ICollection<DebitNoteItem> Items { get; set; } = new List<DebitNoteItem>();
    }
} 