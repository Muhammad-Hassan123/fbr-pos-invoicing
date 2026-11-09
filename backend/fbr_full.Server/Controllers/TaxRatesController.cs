using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fbr_full.Server.Data;
using fbr_full.Server.Models;

namespace fbr_full.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaxRatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaxRatesController> _logger;

        public TaxRatesController(ApplicationDbContext context, ILogger<TaxRatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxRate>>> GetAll()
        {
            var taxRates = await _context.TaxRates.OrderByDescending(t => t.Id).ToListAsync();
            return Ok(taxRates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaxRate>> GetById(int id)
        {
            var taxRate = await _context.TaxRates.FindAsync(id);
            if (taxRate == null) return NotFound();
            return Ok(taxRate);
        }

        [HttpPost]
        public async Task<ActionResult<TaxRate>> Create([FromBody] TaxRate taxRate)
        {
            taxRate.CreatedAt = DateTime.UtcNow;
            _context.TaxRates.Add(taxRate);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = taxRate.Id }, taxRate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxRate updated)
        {
            var taxRate = await _context.TaxRates.FindAsync(id);
            if (taxRate == null) return NotFound();
            taxRate.Name = updated.Name;
            taxRate.Percentage = updated.Percentage;
            taxRate.IsActive = updated.IsActive;
            taxRate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var taxRate = await _context.TaxRates.FindAsync(id);
            if (taxRate == null) return NotFound();
            _context.TaxRates.Remove(taxRate);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}