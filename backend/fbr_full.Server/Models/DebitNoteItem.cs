namespace fbr_full.Server.Models
{
    public class DebitNoteItem
    {
        public int Id { get; set; }
        public int DebitNoteId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal TaxPercentage { get; set; } = 0;
        public decimal Amount { get; set; }
        public decimal Discount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual DebitNote DebitNote { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
} 