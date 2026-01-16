using DoohAdManager.Data;
using DoohAdManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoohAdManager.Controllers
{
    [Route("api/screens")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ScreensController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/screens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Screen>>> GetScreens()
        {
            return await _context.Screens.ToListAsync();
        }
        
        // GET: api/screens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Screen>> GetScreen(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            
            if (screen == null) return NotFound();
            return screen;
        }
        
        // POST: api/screens
        [HttpPost]
        public async Task<ActionResult<Screen>> PostScreen(Screen screen)
        {
            _context.Screens.Add(screen);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetScreen), new { id = screen.Id }, screen);
        }
        
        // PUT: api/screens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScreen(int id, Screen screen)
        {
            if (id != screen.Id) return BadRequest();
            
            _context.Entry(screen).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScreenExists(id)) return NotFound();
                throw;
            }
            
            return NoContent();
        }
        
        // PATCH: api/screens/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateScreenStatus(int id, [FromBody] bool isActive)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null) return NotFound();
            
            screen.IsActive = isActive;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        // DELETE: api/screens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null) return NotFound();
            
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        private bool ScreenExists(int id) => _context.Screens.Any(e => e.Id == id);
    }
}