using DoohAdManager.Data;
using DoohAdManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoohAdManager.Controllers
{
    [Route("api/proofofplay")]
    [ApiController]
    public class ProofOfPlayController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ProofOfPlayController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/proofofplay?screenId=1&startDate=2025-01-01&endDate=2025-01-15
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProofOfPlay>>> GetProofOfPlay(
            [FromQuery] int? screenId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = _context.ProofOfPlays.AsQueryable();
            
            if (screenId.HasValue)
                query = query.Where(p => p.ScreenId == screenId.Value);
            
            if (startDate.HasValue)
                query = query.Where(p => p.PlayedAt >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(p => p.PlayedAt <= endDate.Value);
            
            var results = await query
                .OrderByDescending(p => p.PlayedAt)
                .ToListAsync();
            
            return Ok(results);
        }
    }
}