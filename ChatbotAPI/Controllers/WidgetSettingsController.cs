using Microsoft.AspNetCore.Mvc;
using ChatbotAPI.Models;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WidgetSettingsController : ControllerBase
    {
        private readonly ChatbotDbContext _context;

        public WidgetSettingsController(ChatbotDbContext context)
        {
            _context = context;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetSettings(Guid projectId)
        {
            var settings = await _context.WidgetSettings.FirstOrDefaultAsync(ws => ws.ProjectId == projectId);
            if (settings == null)
                return NotFound();

            return Ok(settings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSettings([FromBody] WidgetSettings newSettings)
        {
            _context.WidgetSettings.Add(newSettings);
            await _context.SaveChangesAsync();
            return Ok(newSettings);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateSettings(Guid projectId, [FromBody] WidgetSettings updated)
        {
            var existing = await _context.WidgetSettings.FirstOrDefaultAsync(ws => ws.ProjectId == projectId);
            if (existing == null)
                return NotFound();

            existing.InactiveMessage = updated.InactiveMessage;
            existing.PrimaryColor = updated.PrimaryColor;
            existing.SecondaryColor = updated.SecondaryColor;
            existing.UseGradient = updated.UseGradient;
            existing.CornerRadius = updated.CornerRadius;
            existing.ScriptTag = updated.ScriptTag;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }
    }
}
