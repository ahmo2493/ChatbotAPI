using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ChatbotDbContext _context;
        public ProjectsController(ChatbotDbContext ctx) => _context = ctx;

        public record CreateProjectDto(string? Name);

        private bool TryGetUserId(out int userId)
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idStr, out userId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto? dto)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user is null) return Unauthorized();

            var p = new Project
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user,
                Name = "Draft"
            };

            _context.Projects.Add(p);
            await _context.SaveChangesAsync();
            return Ok(new { p.Id, p.Name, p.CreatedAt });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var p = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            return p is null ? NotFound() : Ok(p);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var items = await _context.Projects
                .Where(p => p.UserId == userId)  // ⬅️ only current user’s projects
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.CreatedAt,
                    p.UpdatedAt,
                    UserEmail = p.User.Email
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var p = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (p is null) return NotFound();

            _context.Projects.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
