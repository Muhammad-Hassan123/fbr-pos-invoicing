using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fbr_full.Server.Data;
using fbr_full.Server.DTOs;
using fbr_full.Server.Models;

namespace fbr_full.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ApplicationDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CustomerDto>>> GetCustomers([FromQuery] CustomerSearchRequest request)
        {
            try
            {
                var query = _context.Customers.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(c => 
                        c.CustomerName.Contains(request.SearchTerm) ||
                        c.Email!.Contains(request.SearchTerm) ||
                        c.Phone!.Contains(request.SearchTerm) ||
                        c.NTN!.Contains(request.SearchTerm)
                    );
                }

                if (!string.IsNullOrEmpty(request.Province))
                {
                    query = query.Where(c => c.Province == request.Province);
                }

                if (request.IsActive.HasValue)
                {
                    query = query.Where(c => c.IsActive == request.IsActive.Value);
                }

                // Get total count
                var totalCount = await query.CountAsync();

                // Apply pagination
                var customers = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        CustomerName = c.CustomerName,
                        Email = c.Email,
                        Phone = c.Phone,
                        Address = c.Address,
                        Province = c.Province,
                        NTN = c.NTN,
                        RegistrationNumber = c.RegistrationNumber,
                        TaxId = c.TaxId,
                        Scenario = c.Scenario,
                        Description = c.Description,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync();

                var result = new PagedResult<CustomerDto>
                {
                    Items = customers,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                return StatusCode(500, new { message = "An error occurred while getting customers" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .Where(c => c.Id == id)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        CustomerName = c.CustomerName,
                        Email = c.Email,
                        Phone = c.Phone,
                        Address = c.Address,
                        Province = c.Province,
                        NTN = c.NTN,
                        RegistrationNumber = c.RegistrationNumber,
                        TaxId = c.TaxId,
                        Scenario = c.Scenario,
                        Description = c.Description,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer {CustomerId}", id);
                return StatusCode(500, new { message = "An error occurred while getting customer" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                var customer = new Customer
                {
                    CustomerName = request.CustomerName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    Province = request.Province,
                    NTN = request.NTN,
                    RegistrationNumber = request.RegistrationNumber,
                    TaxId = request.TaxId,
                    Scenario = request.Scenario,
                    Description = request.Description,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                var customerDto = new CustomerDto
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Province = customer.Province,
                    NTN = customer.NTN,
                    RegistrationNumber = customer.RegistrationNumber,
                    TaxId = customer.TaxId,
                    Scenario = customer.Scenario,
                    Description = customer.Description,
                    IsActive = customer.IsActive,
                    CreatedAt = customer.CreatedAt,
                    UpdatedAt = customer.UpdatedAt
                };

                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(500, new { message = "An error occurred while creating customer" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                customer.CustomerName = request.CustomerName;
                customer.Email = request.Email;
                customer.Phone = request.Phone;
                customer.Address = request.Address;
                customer.Province = request.Province;
                customer.NTN = request.NTN;
                customer.RegistrationNumber = request.RegistrationNumber;
                customer.TaxId = request.TaxId;
                customer.Scenario = request.Scenario;
                customer.Description = request.Description;
                customer.IsActive = request.IsActive;
                customer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var customerDto = new CustomerDto
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Province = customer.Province,
                    NTN = customer.NTN,
                    RegistrationNumber = customer.RegistrationNumber,
                    TaxId = customer.TaxId,
                    Scenario = customer.Scenario,
                    Description = customer.Description,
                    IsActive = customer.IsActive,
                    CreatedAt = customer.CreatedAt,
                    UpdatedAt = customer.UpdatedAt
                };

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer {CustomerId}", id);
                return StatusCode(500, new { message = "An error occurred while updating customer" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                // Check if customer has any invoices
                var hasInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id);
                if (hasInvoices)
                {
                    return BadRequest(new { message = "Cannot delete customer with existing invoices" });
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer {CustomerId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting customer" });
            }
        }

        [HttpGet("provinces")]
        public async Task<ActionResult<List<string>>> GetProvinces()
        {
            try
            {
                var provinces = await _context.Customers
                    .Where(c => !string.IsNullOrEmpty(c.Province))
                    .Select(c => c.Province!)
                    .Distinct()
                    .OrderBy(p => p)
                    .ToListAsync();

                return Ok(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting provinces");
                return StatusCode(500, new { message = "An error occurred while getting provinces" });
            }
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
} 