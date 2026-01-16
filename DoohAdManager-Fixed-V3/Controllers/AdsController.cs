using DoohAdManager.Data;
using DoohAdManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoohAdManager.Controllers
{
    [Route("api/ads")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AdsController(
            ApplicationDbContext context, 
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        
        // GET: api/ads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
        {
            var ads = await _context.Ads
                .AsNoTracking()
                .ToListAsync();
            
            // Convert relative URLs to absolute URLs
            var baseUrl = GetBaseUrl();
            Console.WriteLine($"GetAds - Base URL: {baseUrl}");
            
            foreach (var ad in ads)
            {
                if (!string.IsNullOrEmpty(ad.MediaUrl) && !ad.MediaUrl.StartsWith("http"))
                {
                    ad.MediaUrl = $"{baseUrl}{ad.MediaUrl}";
                }
                Console.WriteLine($"Ad {ad.Id}: {ad.MediaUrl}");
            }
            
            return Ok(ads);
        }
        
        // POST: api/ads/upload
        [HttpPost("upload")]
        public async Task<ActionResult<Ad>> UploadAd(
            [FromForm] string name,
            [FromForm] int duration,
            [FromForm] IFormFile file)
        {
            Console.WriteLine($"Upload request received: {name}, {duration}s");
            
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Name is required" });

            if (duration <= 0)
                return BadRequest(new { message = "Duration must be greater than 0" });

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp", "video/mp4", "video/webm", "video/ogg" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { message = "Invalid file type. Only images and videos are allowed." });

            // Save to wwwroot/uploads (standard .NET location)
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            Console.WriteLine($"Saving file to: {filePath}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Console.WriteLine($"File saved. Size: {new FileInfo(filePath).Length} bytes");

            var baseUrl = GetBaseUrl();
            var ad = new Ad
            {
                Title = name,
                DurationSeconds = duration,
                MediaType = file.ContentType,
                MediaUrl = $"{baseUrl}/uploads/{fileName}"
            };

            Console.WriteLine($"Generated URL: {ad.MediaUrl}");

            _context.Ads.Add(ad);
            await _context.SaveChangesAsync();

            return Ok(ad);
        }

        // DELETE: api/ads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(int id)
        {
            var ad = await _context.Ads.FindAsync(id);
            if (ad == null) 
                return NotFound(new { message = "Ad not found" });
            
            // Delete file from filesystem
            try
            {
                var uri = new Uri(ad.MediaUrl);
                var fileName = Path.GetFileName(uri.LocalPath);
                var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Deleted file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
            
            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        // Helper method to get base URL
        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;
            
            return $"{request.Scheme}://{request.Host}";
        }
    }
}
