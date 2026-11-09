using System.ComponentModel.DataAnnotations;

namespace fbr_full.Server.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? ReferencePurchase { get; set; }
        public string? DeliveryAddress { get; set; }
        public string SaleType { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FbrReferenceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal WithholdingTaxPercentage { get; set; }
        public decimal ExtraTaxPercentage { get; set; }
        public decimal FurtherTaxPercentage { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public decimal RetailAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AmountWithTax { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class PaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class CreateInvoiceRequest
    {
        [Required]
        public string CustomerType { get; set; } = string.Empty; // Registered Customer, Walking Customer

        public int? CustomerId { get; set; }

        [StringLength(100)]
        public string? ReferencePurchase { get; set; }

        [StringLength(500)]
        public string? DeliveryAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string SaleType { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Note { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InvoiceDiscount { get; set; } = 0;

        [Required]
        public List<CreateInvoiceItemRequest> Items { get; set; } = new();
    }

    public class CreateInvoiceItemRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Rate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RetailPrice { get; set; }

        [Range(0, 100)]
        public decimal TaxPercentage { get; set; } = 0;

        [Range(0, 100)]
        public decimal WithholdingTaxPercentage { get; set; } = 0;

        [Range(0, 100)]
        public decimal ExtraTaxPercentage { get; set; } = 0;

        [Range(0, 100)]
        public decimal FurtherTaxPercentage { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; } = 0;
    }

    public class UpdateInvoiceRequest : CreateInvoiceRequest
    {
        public string Status { get; set; } = "Draft";
    }

    public class InvoiceSearchRequest
    {
        public string? SearchTerm { get; set; }
        public string? CustomerType { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class FbrInvoiceRequest
    {
        public int InvoiceId { get; set; }
        public string PosId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class FbrInvoiceResponse
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string FbrReferenceNumber { get; set; } = string.Empty;
        public string? ResponseData { get; set; }
        public string Status { get; set; } = string.Empty; // Success, Failed, Pending
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Additional properties used by the service
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

} 