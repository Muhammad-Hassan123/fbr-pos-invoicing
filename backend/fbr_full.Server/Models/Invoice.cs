namespace fbr_full.Server.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty; // Registered Customer, Walking Customer
        public int? CustomerId { get; set; }
        public string? ReferencePurchase { get; set; }
        public string? DeliveryAddress { get; set; }
        public string SaleType { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceDiscount { get; set; } = 0;
        public decimal SubTotal { get; set; } = 0;
        public decimal TotalTax { get; set; } = 0;
        public decimal TotalDiscount { get; set; } = 0;
        public decimal GrandTotal { get; set; } = 0;
        public string Status { get; set; } = "Draft"; // Draft, Pending, Completed, Cancelled
        public string? FbrReferenceNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<DebitNote> DebitNotes { get; set; } = new List<DebitNote>();
        public virtual ICollection<FbrInvoiceResponse> FbrResponses { get; set; } = new List<FbrInvoiceResponse>();
    }
} 