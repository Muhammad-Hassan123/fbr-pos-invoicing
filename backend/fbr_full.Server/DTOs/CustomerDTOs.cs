using System.ComponentModel.DataAnnotations;

namespace fbr_full.Server.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? NTN { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? TaxId { get; set; }
        public string? Scenario { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCustomerRequest
    {
        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? Province { get; set; }

        [StringLength(50)]
        public string? NTN { get; set; }

        [StringLength(50)]
        public string? RegistrationNumber { get; set; }

        [StringLength(50)]
        public string? TaxId { get; set; }

        [StringLength(50)]
        public string? Scenario { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateCustomerRequest : CreateCustomerRequest
    {
        public bool IsActive { get; set; } = true;
    }

    public class CustomerSearchRequest
    {
        public string? SearchTerm { get; set; }
        public string? Province { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 