using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fbr_full.Server.Data;
using fbr_full.Server.DTOs;
using fbr_full.Server.Models;
using fbr_full.Server.Services;

namespace fbr_full.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFbrApiService _fbrApiService;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(
            ApplicationDbContext context,
            IFbrApiService fbrApiService,
            ILogger<InvoicesController> logger)
        {
            _context = context;
            _fbrApiService = fbrApiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<InvoiceDto>>> GetInvoices([FromQuery] InvoiceSearchRequest request)
        {
            try
            {
                var query = _context.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(i => 
                        i.InvoiceNumber.Contains(request.SearchTerm) ||
                        i.Customer!.CustomerName.Contains(request.SearchTerm) ||
                        i.FbrReferenceNumber!.Contains(request.SearchTerm)
                    );
                }

                if (!string.IsNullOrEmpty(request.CustomerType))
                {
                    query = query.Where(i => i.CustomerType == request.CustomerType);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(i => i.Status == request.Status);
                }

                if (request.FromDate.HasValue)
                {
                    query = query.Where(i => i.IssueDate >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    query = query.Where(i => i.IssueDate <= request.ToDate.Value);
                }

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply pagination
                var invoices = await query
                    .OrderByDescending(i => i.CreatedAt)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(i => new InvoiceDto
                    {
                        Id = i.Id,
                        InvoiceNumber = i.InvoiceNumber,
                        CustomerType = i.CustomerType,
                        CustomerId = i.CustomerId,
                        CustomerName = i.Customer!.CustomerName,
                        ReferencePurchase = i.ReferencePurchase,
                        DeliveryAddress = i.DeliveryAddress,
                        SaleType = i.SaleType,
                        Note = i.Note,
                        IssueDate = i.IssueDate,
                        DueDate = i.DueDate,
                        InvoiceDiscount = i.InvoiceDiscount,
                        SubTotal = i.SubTotal,
                        TotalTax = i.TotalTax,
                        TotalDiscount = i.TotalDiscount,
                        GrandTotal = i.GrandTotal,
                        Status = i.Status,
                        FbrReferenceNumber = i.FbrReferenceNumber,
                        CreatedAt = i.CreatedAt,
                        UpdatedAt = i.UpdatedAt,
                        Items = i.Items.Select(ii => new InvoiceItemDto
                        {
                            Id = ii.Id,
                            ProductId = ii.ProductId,
                            ProductName = ii.Product.Name,
                            ProductSKU = ii.Product.SKU,
                            Quantity = ii.Quantity,
                            Rate = ii.Rate,
                            RetailPrice = ii.RetailPrice,
                            TaxPercentage = ii.TaxPercentage,
                            WithholdingTaxPercentage = ii.WithholdingTaxPercentage,
                            ExtraTaxPercentage = ii.ExtraTaxPercentage,
                            FurtherTaxPercentage = ii.FurtherTaxPercentage,
                            Discount = ii.Discount,
                            Amount = ii.Amount,
                            RetailAmount = ii.RetailAmount,
                            TaxAmount = ii.TaxAmount,
                            AmountWithTax = ii.AmountWithTax,
                            NetAmount = ii.NetAmount
                        }).ToList(),
                        Payments = i.Payments.Select(p => new PaymentDto
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            PaymentMethod = p.PaymentMethod,
                            ReferenceNumber = p.ReferenceNumber,
                            Notes = p.Notes,
                            PaymentDate = p.PaymentDate
                        }).ToList()
                    })
                    .ToListAsync();

                var result = new PagedResult<InvoiceDto>
                {
                    Items = invoices,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoices");
                return StatusCode(500, new { message = "An error occurred while getting invoices" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                    .Include(i => i.Payments)
                    .Where(i => i.Id == id)
                    .Select(i => new InvoiceDto
                    {
                        Id = i.Id,
                        InvoiceNumber = i.InvoiceNumber,
                        CustomerType = i.CustomerType,
                        CustomerId = i.CustomerId,
                        CustomerName = i.Customer!.CustomerName,
                        ReferencePurchase = i.ReferencePurchase,
                        DeliveryAddress = i.DeliveryAddress,
                        SaleType = i.SaleType,
                        Note = i.Note,
                        IssueDate = i.IssueDate,
                        DueDate = i.DueDate,
                        InvoiceDiscount = i.InvoiceDiscount,
                        SubTotal = i.SubTotal,
                        TotalTax = i.TotalTax,
                        TotalDiscount = i.TotalDiscount,
                        GrandTotal = i.GrandTotal,
                        Status = i.Status,
                        FbrReferenceNumber = i.FbrReferenceNumber,
                        CreatedAt = i.CreatedAt,
                        UpdatedAt = i.UpdatedAt,
                        Items = i.Items.Select(ii => new InvoiceItemDto
                        {
                            Id = ii.Id,
                            ProductId = ii.ProductId,
                            ProductName = ii.Product.Name,
                            ProductSKU = ii.Product.SKU,
                            Quantity = ii.Quantity,
                            Rate = ii.Rate,
                            RetailPrice = ii.RetailPrice,
                            TaxPercentage = ii.TaxPercentage,
                            WithholdingTaxPercentage = ii.WithholdingTaxPercentage,
                            ExtraTaxPercentage = ii.ExtraTaxPercentage,
                            FurtherTaxPercentage = ii.FurtherTaxPercentage,
                            Discount = ii.Discount,
                            Amount = ii.Amount,
                            RetailAmount = ii.RetailAmount,
                            TaxAmount = ii.TaxAmount,
                            AmountWithTax = ii.AmountWithTax,
                            NetAmount = ii.NetAmount
                        }).ToList(),
                        Payments = i.Payments.Select(p => new PaymentDto
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            PaymentMethod = p.PaymentMethod,
                            ReferenceNumber = p.ReferenceNumber,
                            Notes = p.Notes,
                            PaymentDate = p.PaymentDate
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (invoice == null)
                {
                    return NotFound();
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoice {InvoiceId}", id);
                return StatusCode(500, new { message = "An error occurred while getting invoice" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            try
            {
                // Generate invoice number
                var lastInvoice = await _context.Invoices
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefaultAsync();

                var invoiceNumber = lastInvoice != null 
                    ? $"INV{(lastInvoice.Id + 1):D8}"
                    : "INV00000001";

                var invoice = new Invoice
                {
                    InvoiceNumber = invoiceNumber,
                    CustomerType = request.CustomerType,
                    CustomerId = request.CustomerId,
                    ReferencePurchase = request.ReferencePurchase,
                    DeliveryAddress = request.DeliveryAddress,
                    SaleType = request.SaleType,
                    Note = request.Note,
                    IssueDate = request.IssueDate,
                    DueDate = request.DueDate,
                    InvoiceDiscount = request.InvoiceDiscount,
                    Status = "Draft",
                    CreatedAt = DateTime.UtcNow
                };

                // Calculate totals
                decimal subTotal = 0;
                decimal totalTax = 0;
                decimal totalDiscount = 0;

                foreach (var itemRequest in request.Items)
                {
                    var product = await _context.Products.FindAsync(itemRequest.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"Product with ID {itemRequest.ProductId} not found" });
                    }

                    var amount = itemRequest.Quantity * itemRequest.Rate;
                    var retailAmount = itemRequest.Quantity * itemRequest.RetailPrice;
                    var taxAmount = amount * (itemRequest.TaxPercentage / 100);
                    var amountWithTax = amount + taxAmount;
                    var netAmount = amountWithTax - itemRequest.Discount;

                    var invoiceItem = new InvoiceItem
                    {
                        ProductId = itemRequest.ProductId,
                        Quantity = itemRequest.Quantity,
                        Rate = itemRequest.Rate,
                        RetailPrice = itemRequest.RetailPrice,
                        TaxPercentage = itemRequest.TaxPercentage,
                        WithholdingTaxPercentage = itemRequest.WithholdingTaxPercentage,
                        ExtraTaxPercentage = itemRequest.ExtraTaxPercentage,
                        FurtherTaxPercentage = itemRequest.FurtherTaxPercentage,
                        Discount = itemRequest.Discount,
                        Amount = amount,
                        RetailAmount = retailAmount,
                        TaxAmount = taxAmount,
                        AmountWithTax = amountWithTax,
                        NetAmount = netAmount,
                        CreatedAt = DateTime.UtcNow
                    };

                    invoice.Items.Add(invoiceItem);

                    subTotal += amount;
                    totalTax += taxAmount;
                    totalDiscount += itemRequest.Discount;
                }

                invoice.SubTotal = subTotal;
                invoice.TotalTax = totalTax;
                invoice.TotalDiscount = totalDiscount + request.InvoiceDiscount;
                invoice.GrandTotal = subTotal + totalTax - invoice.TotalDiscount;

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                // Return the created invoice
                return await GetInvoice(invoice.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return StatusCode(500, new { message = "An error occurred while creating invoice" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoice(int id, [FromBody] UpdateInvoiceRequest request)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Items)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (invoice == null)
                {
                    return NotFound();
                }

                if (invoice.Status == "Completed")
                {
                    return BadRequest(new { message = "Cannot update completed invoice" });
                }

                // Update basic invoice information
                invoice.CustomerType = request.CustomerType;
                invoice.CustomerId = request.CustomerId;
                invoice.ReferencePurchase = request.ReferencePurchase;
                invoice.DeliveryAddress = request.DeliveryAddress;
                invoice.SaleType = request.SaleType;
                invoice.Note = request.Note;
                invoice.IssueDate = request.IssueDate;
                invoice.DueDate = request.DueDate;
                invoice.InvoiceDiscount = request.InvoiceDiscount;
                invoice.Status = request.Status;
                invoice.UpdatedAt = DateTime.UtcNow;

                // Remove existing items
                _context.InvoiceItems.RemoveRange(invoice.Items);
                invoice.Items.Clear();

                // Recalculate totals
                decimal subTotal = 0;
                decimal totalTax = 0;
                decimal totalDiscount = 0;

                foreach (var itemRequest in request.Items)
                {
                    var product = await _context.Products.FindAsync(itemRequest.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"Product with ID {itemRequest.ProductId} not found" });
                    }

                    var amount = itemRequest.Quantity * itemRequest.Rate;
                    var retailAmount = itemRequest.Quantity * itemRequest.RetailPrice;
                    var taxAmount = amount * (itemRequest.TaxPercentage / 100);
                    var amountWithTax = amount + taxAmount;
                    var netAmount = amountWithTax - itemRequest.Discount;

                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ProductId = itemRequest.ProductId,
                        Quantity = itemRequest.Quantity,
                        Rate = itemRequest.Rate,
                        RetailPrice = itemRequest.RetailPrice,
                        TaxPercentage = itemRequest.TaxPercentage,
                        WithholdingTaxPercentage = itemRequest.WithholdingTaxPercentage,
                        ExtraTaxPercentage = itemRequest.ExtraTaxPercentage,
                        FurtherTaxPercentage = itemRequest.FurtherTaxPercentage,
                        Discount = itemRequest.Discount,
                        Amount = amount,
                        RetailAmount = retailAmount,
                        TaxAmount = taxAmount,
                        AmountWithTax = amountWithTax,
                        NetAmount = netAmount,
                        CreatedAt = DateTime.UtcNow
                    };

                    invoice.Items.Add(invoiceItem);

                    subTotal += amount;
                    totalTax += taxAmount;
                    totalDiscount += itemRequest.Discount;
                }

                invoice.SubTotal = subTotal;
                invoice.TotalTax = totalTax;
                invoice.TotalDiscount = totalDiscount + request.InvoiceDiscount;
                invoice.GrandTotal = subTotal + totalTax - invoice.TotalDiscount;

                await _context.SaveChangesAsync();

                return await GetInvoice(invoice.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice {InvoiceId}", id);
                return StatusCode(500, new { message = "An error occurred while updating invoice" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (invoice == null)
                {
                    return NotFound();
                }

                if (invoice.Status == "Completed")
                {
                    return BadRequest(new { message = "Cannot delete completed invoice" });
                }

                if (invoice.Payments.Any())
                {
                    return BadRequest(new { message = "Cannot delete invoice with payments" });
                }

                _context.InvoiceItems.RemoveRange(invoice.Items);
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice {InvoiceId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting invoice" });
            }
        }

        [HttpPost("{id}/submit-to-fbr")]
        public async Task<ActionResult<DTOs.FbrInvoiceResponse>> SubmitToFbr(int id, [FromBody] FbrInvoiceRequest request)
        {
            try
            {
                var invoice = await _context.Invoices.FindAsync(id);
                if (invoice == null)
                {
                    return NotFound();
                }

                if (invoice.Status != "Draft" && invoice.Status != "Pending")
                {
                    return BadRequest(new { message = "Invoice must be in Draft or Pending status to submit to FBR" });
                }

                var response = await _fbrApiService.SubmitInvoiceToFbrAsync(
                    id, 
                    request.PosId, 
                    request.Username, 
                    request.Password
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting invoice {InvoiceId} to FBR", id);
                return StatusCode(500, new { message = "An error occurred while submitting to FBR" });
            }
        }

        [HttpGet("statuses")]
        public ActionResult<List<string>> GetInvoiceStatuses()
        {
            return Ok(new List<string> { "Draft", "Pending", "Completed", "Cancelled" });
        }

        [HttpGet("customer-types")]
        public ActionResult<List<string>> GetCustomerTypes()
        {
            return Ok(new List<string> { "Registered Customer", "Walking Customer" });
        }

        [HttpGet("sale-types")]
        public ActionResult<List<string>> GetSaleTypes()
        {
            return Ok(new List<string> { "Retail", "Wholesale", "Export", "Local" });
        }
    }
} 