using System.ComponentModel.DataAnnotations;

namespace fbr_full.Server.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? HSCode { get; set; }
        public string UOM { get; set; } = string.Empty;
        public decimal? UnitPrice { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateProductRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(50)]
        public string? HSCode { get; set; }

        [Required]
        [StringLength(20)]
        public string UOM { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal? UnitPrice { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
    }

    public class UpdateProductRequest : CreateProductRequest
    {
        public bool IsActive { get; set; } = true;
    }

    public class ProductSearchRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 