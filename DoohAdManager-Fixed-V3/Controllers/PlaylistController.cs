using DoohAdManager.Data;
using DoohAdManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoohAdManager.Controllers
{
    [Route("api/screens")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public PlaylistController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/screens/{screenId}/playlist?at=2025-01-01T10:30:00Z
        [HttpGet("{screenId}/playlist")]
        public async Task<ActionResult<IEnumerable<PlaylistItem>>> GetPlaylist(
            int screenId,
            [FromQuery] DateTime at)
        {
            // Convert to UTC
            var queryTime = at.ToUniversalTime();
            
            // Find active campaign for this screen at given time
            var campaign = await _context.CampaignScreens
                .Where(cs => cs.ScreenId == screenId)
                .Select(cs => cs.Campaign)
                .Where(c => 
                    c.StartTime <= queryTime && 
                    c.EndTime >= queryTime)
                .FirstOrDefaultAsync();
            
            if (campaign == null)
                return NotFound("No active campaign for this screen at specified time");
            
            // Get ads with play_order
            var playlist = await _context.CampaignAds
                .Where(ca => ca.CampaignId == campaign.Id)
                .Include(ca => ca.Ad)
                .OrderBy(ca => ca.PlayOrder) // REQUIRED: ordered by play_order
                .Select(ca => new PlaylistItem
                {
                    AdId = ca.AdId,
                    Title = ca.Ad.Title,
                    MediaUrl = ca.Ad.MediaUrl,
                    DurationSeconds = ca.Ad.DurationSeconds,
                    MediaType = ca.Ad.MediaType,
                    PlayOrder = ca.PlayOrder
                })
                .ToListAsync();
            
            return playlist;
        }
        
        // POST: api/proofofplay (for completeness)
        [HttpPost("proofofplay")]
        public async Task<IActionResult> RecordProofOfPlay(ProofOfPlayRequest request)
        {
            var proof = new ProofOfPlay
            {
                ScreenId = request.ScreenId,
                AdId = request.AdId,
                PlayedAt = DateTime.UtcNow
            };
            
            _context.ProofOfPlays.Add(proof);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Playback recorded", id = proof.Id });
        }
        
        // Response DTO
        public class PlaylistItem
        {
            public int AdId { get; set; }
            public required string Title { get; set; }
            public required string MediaUrl { get; set; }
            public int DurationSeconds { get; set; }
            public required string MediaType { get; set; }
            public int PlayOrder { get; set; }
        }
        
        public class ProofOfPlayRequest
        {
            public int ScreenId { get; set; }
            public int AdId { get; set; }
        }
    }
}