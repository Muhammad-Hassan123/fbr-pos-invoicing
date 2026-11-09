using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using fbr_full.Server.Models;

namespace fbr_full.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Core POS Entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteItem> DebitNoteItems { get; set; }

        // FBR Integration Entities
        public DbSet<FbrApiLog> FbrApiLogs { get; set; }
        public DbSet<FbrInvoiceResponse> FbrInvoiceResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints
            builder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.NTN).HasMaxLength(50);
                entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
                entity.Property(e => e.TaxId).HasMaxLength(50);
                entity.Property(e => e.Province).HasMaxLength(50);
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CostPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StockQuantity).HasColumnType("decimal(10,2)");
                entity.Property(e => e.MinStockLevel).HasColumnType("decimal(10,2)");
                entity.Property(e => e.MaxStockLevel).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            builder.Entity<TaxRate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Percentage).HasColumnType("decimal(5,2)");
            });

            builder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CustomerType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ReferencePurchase).HasMaxLength(100);
                entity.Property(e => e.DeliveryAddress).HasMaxLength(500);
                entity.Property(e => e.SaleType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalTax).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalDiscount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GrandTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FbrReferenceNumber).HasMaxLength(100);
                entity.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
            });

            builder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.WithholdingTaxPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.ExtraTaxPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.FurtherTaxPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RetailAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AmountWithTax).HasColumnType("decimal(18,2)");
                entity.Property(e => e.NetAmount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Invoice).WithMany(i => i.Items).HasForeignKey(e => e.InvoiceId);
                entity.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.HasOne(e => e.Invoice).WithMany().HasForeignKey(e => e.InvoiceId);
            });

            builder.Entity<DebitNote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DebitNoteNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.InvoiceDiscount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalTax).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalDiscount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GrandTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.HasOne(e => e.Invoice).WithMany().HasForeignKey(e => e.InvoiceId);
            });

            builder.Entity<DebitNoteItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TaxPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.DebitNote).WithMany(d => d.Items).HasForeignKey(e => e.DebitNoteId);
                entity.HasOne(e => e.Product).WithMany().HasForeignKey(e => e.ProductId);
            });

            builder.Entity<FbrApiLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Endpoint).IsRequired().HasMaxLength(200);
                entity.Property(e => e.RequestData).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ResponseData).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            });

            builder.Entity<FbrInvoiceResponse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FbrReferenceNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ResponseData).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
                entity.HasOne(e => e.Invoice).WithMany().HasForeignKey(e => e.InvoiceId);
            });

            // Seed initial data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed Tax Rates
            builder.Entity<TaxRate>().HasData(
                new TaxRate { Id = 1, Name = "General Sales Tax", Percentage = 18.00m },
                new TaxRate { Id = 2, Name = "Environmental Tax", Percentage = 5.00m },
                new TaxRate { Id = 3, Name = "Import Duty", Percentage = 12.50m },
                new TaxRate { Id = 4, Name = "Luxury Tax", Percentage = 25.00m },
                new TaxRate { Id = 5, Name = "Service Tax", Percentage = 15.00m },
                new TaxRate { Id = 6, Name = "NA", Percentage = 0.00m }
            );

            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" },
                new Category { Id = 3, Name = "Food & Beverages", Description = "Food and drink products" },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home improvement and garden items" }
            );
        }
    }
} 