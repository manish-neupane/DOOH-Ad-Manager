using DoohAdManager.Data;
using DoohAdManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoohAdManager.Controllers
{
    [Route("api/campaigns")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public CampaignsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/campaigns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns()
        {
            var campaigns = await _context.Campaigns
                .Include(c => c.CampaignScreens)
                .Include(c => c.CampaignAds)
                .AsNoTracking() // Add this to prevent tracking
                .ToListAsync();
            
            return Ok(campaigns);
        }
        
        // GET: api/campaigns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.CampaignScreens)
                .Include(c => c.CampaignAds)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (campaign == null)
            {
                return NotFound();
            }
            
            return Ok(campaign);
        }
        
        // POST: api/campaigns
        [HttpPost]
        public async Task<ActionResult<Campaign>> PostCampaign(CampaignRequest request)
        {
            // Validation
            if (request.StartTime >= request.EndTime)
                return BadRequest(new { message = "Start time must be before end time" });
            
            // Check screen existence and status
            foreach (var screenId in request.ScreenIds)
            {
                var screen = await _context.Screens.FindAsync(screenId);
                if (screen == null)
                    return BadRequest(new { message = $"Screen {screenId} does not exist" });
                
                if (!screen.IsActive)
                    return BadRequest(new { message = $"Screen {screenId} is inactive" });
            }
            
            // Check ad existence
            foreach (var adId in request.AdIds)
            {
                if (!await _context.Ads.AnyAsync(a => a.Id == adId))
                    return BadRequest(new { message = $"Ad {adId} does not exist" });
            }
            
            // Check for scheduling conflicts (REQUIRED: No overlaps)
            foreach (var screenId in request.ScreenIds)
            {
                var hasConflict = await _context.CampaignScreens
                    .Where(cs => cs.ScreenId == screenId)
                    .Select(cs => cs.Campaign)
                    .AnyAsync(c => 
                        c.StartTime < request.EndTime && 
                        c.EndTime > request.StartTime);
                
                if (hasConflict)
                    return BadRequest(new { message = $"Screen {screenId} has scheduling conflict" });
            }
            
            // Create campaign
            var campaign = new Campaign
            {
                Name = request.Name,
                StartTime = request.StartTime.ToUniversalTime(),
                EndTime = request.EndTime.ToUniversalTime()
            };
            
            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();
            
            // Add screens
            foreach (var screenId in request.ScreenIds)
            {
                _context.CampaignScreens.Add(new CampaignScreen
                {
                    CampaignId = campaign.Id,
                    ScreenId = screenId
                });
            }
            
            // Add ads with play_order (REQUIRED)
            for (int i = 0; i < request.AdIds.Count; i++)
            {
                _context.CampaignAds.Add(new CampaignAd
                {
                    CampaignId = campaign.Id,
                    AdId = request.AdIds[i],
                    PlayOrder = i + 1 // Sequence number
                });
            }
            
            await _context.SaveChangesAsync();
            
            // Reload the campaign with its relationships for return
            var createdCampaign = await _context.Campaigns
                .Include(c => c.CampaignScreens)
                .Include(c => c.CampaignAds)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == campaign.Id);
            
            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, createdCampaign);
        }
        
        // PUT: api/campaigns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampaign(int id, CampaignRequest request)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null) 
                return NotFound(new { message = "Campaign not found" });
            
            // Similar validation as POST
            if (request.StartTime >= request.EndTime)
                return BadRequest(new { message = "Start time must be before end time" });
            
            // Remove existing relationships
            var existingScreens = _context.CampaignScreens.Where(cs => cs.CampaignId == id);
            _context.CampaignScreens.RemoveRange(existingScreens);
            
            var existingAds = _context.CampaignAds.Where(ca => ca.CampaignId == id);
            _context.CampaignAds.RemoveRange(existingAds);
            
            // Update campaign
            campaign.Name = request.Name;
            campaign.StartTime = request.StartTime.ToUniversalTime();
            campaign.EndTime = request.EndTime.ToUniversalTime();
            
            // Add new relationships
            foreach (var screenId in request.ScreenIds)
            {
                _context.CampaignScreens.Add(new CampaignScreen
                {
                    CampaignId = id,
                    ScreenId = screenId
                });
            }
            
            for (int i = 0; i < request.AdIds.Count; i++)
            {
                _context.CampaignAds.Add(new CampaignAd
                {
                    CampaignId = id,
                    AdId = request.AdIds[i],
                    PlayOrder = i + 1
                });
            }
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        // DELETE: api/campaigns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null) 
                return NotFound(new { message = "Campaign not found" });
            
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        // Request DTO
        public class CampaignRequest
        {
            public required string Name { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public required List<int> ScreenIds { get; set; }
            public required List<int> AdIds { get; set; }
        }
    }
}