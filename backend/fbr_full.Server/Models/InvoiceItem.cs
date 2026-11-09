namespace fbr_full.Server.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal TaxPercentage { get; set; } = 0;
        public decimal WithholdingTaxPercentage { get; set; } = 0;
        public decimal ExtraTaxPercentage { get; set; } = 0;
        public decimal FurtherTaxPercentage { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal Amount { get; set; }
        public decimal RetailAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AmountWithTax { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Invoice Invoice { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
} 