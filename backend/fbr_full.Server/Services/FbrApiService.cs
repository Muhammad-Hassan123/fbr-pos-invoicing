using System.Text;
using System.Text.Json;
using fbr_full.Server.Data;
using fbr_full.Server.DTOs;
using fbr_full.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace fbr_full.Server.Services
{
    public interface IFbrApiService
    {
        Task<DTOs.FbrInvoiceResponse> SubmitInvoiceToFbrAsync(int invoiceId, string posId, string username, string password);
        Task<List<FbrApiLog>> GetApiLogsAsync(int page = 1, int pageSize = 10);
    }

    public class FbrApiService : IFbrApiService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FbrApiService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public FbrApiService(ApplicationDbContext context, ILogger<FbrApiService> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<DTOs.FbrInvoiceResponse> SubmitInvoiceToFbrAsync(int invoiceId, string posId, string username, string password)
        {
            var startTime = DateTime.UtcNow;
            var apiLog = new FbrApiLog
            {
                Endpoint = "FBR_INVOICE_SUBMISSION",
                Status = "Pending",
                CreatedAt = startTime
            };

            try
            {
                // Get invoice with all related data
                var invoice = await _context.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Items)
                    .ThenInclude(ii => ii.Product)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice == null)
                {
                    throw new ArgumentException($"Invoice with ID {invoiceId} not found");
                }

                // Prepare FBR request payload
                var fbrRequest = PrepareFbrRequest(invoice, posId, username, password);
                var requestJson = JsonSerializer.Serialize(fbrRequest, new JsonSerializerOptions { WriteIndented = true });

                apiLog.RequestData = requestJson;

                // In production, this would be the actual FBR API endpoint
                // For now, we'll simulate the API call
                var fbrResponse = await SimulateFbrApiCallAsync(fbrRequest);

                var responseTime = DateTime.UtcNow - startTime;
                apiLog.ResponseTime = responseTime;
                apiLog.ResponseData = JsonSerializer.Serialize(fbrResponse, new JsonSerializerOptions { WriteIndented = true });

                if (fbrResponse.Success)
                {
                    apiLog.Status = "Success";

                    // Update invoice with FBR reference number
                    invoice.FbrReferenceNumber = fbrResponse.FbrReferenceNumber;
                    invoice.Status = "Completed";
                    invoice.UpdatedAt = DateTime.UtcNow;

                    // Save FBR response
                    var fbrInvoiceResponse = new Models.FbrInvoiceResponse
                    {
                        InvoiceId = invoiceId,
                        FbrReferenceNumber = fbrResponse.FbrReferenceNumber!,
                        ResponseData = apiLog.ResponseData,
                        Status = "Success",
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.FbrInvoiceResponses.Add(fbrInvoiceResponse);
                }
                else
                {
                    apiLog.Status = "Failed";
                    apiLog.ErrorMessage = fbrResponse.ErrorMessage;
                }

                _context.FbrApiLogs.Add(apiLog);
                await _context.SaveChangesAsync();

                return fbrResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting invoice {InvoiceId} to FBR", invoiceId);

                apiLog.Status = "Failed";
                apiLog.ErrorMessage = ex.Message;
                apiLog.ResponseTime = DateTime.UtcNow - startTime;

                _context.FbrApiLogs.Add(apiLog);
                await _context.SaveChangesAsync();

                return new DTOs.FbrInvoiceResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        public async Task<List<FbrApiLog>> GetApiLogsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.FbrApiLogs
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private object PrepareFbrRequest(Invoice invoice, string posId, string username, string password)
        {
            // This is a sample FBR request structure
            // In production, you would need to follow the exact FBR API specification
            return new
            {
                posID = posId,
                username = username,
                password = password,
                invoice = new
                {
                    invoiceNumber = invoice.InvoiceNumber,
                    invoiceDate = invoice.IssueDate.ToString("yyyy-MM-dd"),
                    buyerName = invoice.Customer?.CustomerName ?? "Walking Customer",
                    buyerNTN = invoice.Customer?.NTN ?? "",
                    buyerCNIC = invoice.Customer?.TaxId ?? "",
                    buyerPhoneNumber = invoice.Customer?.Phone ?? "",
                    buyerAddress = invoice.Customer?.Address ?? "",
                    saleValue = invoice.SubTotal,
                    taxCharged = invoice.TotalTax,
                    totalAmount = invoice.GrandTotal,
                    paymentMode = "Cash", // This should be dynamic based on payment method
                    items = invoice.Items.Select(item => new
                    {
                        itemName = item.Product.Name,
                        quantity = item.Quantity,
                        rate = item.Rate,
                        value = item.Amount,
                        taxRate = item.TaxPercentage,
                        taxAmount = item.TaxAmount,
                        discount = item.Discount
                    }).ToArray()
                }
            };
        }

        private async Task<DTOs.FbrInvoiceResponse> SimulateFbrApiCallAsync(object request)
        {
            // Simulate API delay
            await Task.Delay(2000);

            // Simulate success/failure based on some condition
            var random = new Random();
            var isSuccess = random.Next(1, 10) > 2; // 80% success rate

            if (isSuccess)
            {
                var fbrRefNumber = $"FBR{DateTime.Now:yyyyMMdd}{random.Next(1000, 9999)}";
                
                return new DTOs.FbrInvoiceResponse
                {
                    Success = true,
                    FbrReferenceNumber = fbrRefNumber,
                    ResponseData = JsonSerializer.Serialize(new { 
                        status = "success", 
                        message = "Invoice submitted successfully",
                        fbrReferenceNumber = fbrRefNumber
                    }),
                    Timestamp = DateTime.UtcNow
                };
            }
            else
            {
                return new DTOs.FbrInvoiceResponse
                {
                    Success = false,
                    ErrorMessage = "FBR API Error: Invalid credentials or network issue",
                    ResponseData = JsonSerializer.Serialize(new { 
                        status = "error", 
                        message = "FBR API Error: Invalid credentials or network issue"
                    }),
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
} 