using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WidgetSettingsController : ControllerBase
    {
        private readonly ChatbotDbContext _context;
        public WidgetSettingsController(ChatbotDbContext context) => _context = context;

        private static WidgetSettingsDto Map(WidgetSettings s) =>
            new WidgetSettingsDto(
                s.Id, s.ProjectId, s.ChatHeaderText, s.InactiveMessage,
                s.PrimaryColor, s.SecondaryColor, s.UseGradient, s.CornerRadius, s.ScriptTag
            );

        [HttpGet("{projectId:guid}")]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            var s = await _context.WidgetSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(ws => ws.ProjectId == projectId);

            return s is null ? NotFound() : Ok(Map(s));
        }

        // POST upsert
        [HttpPost("{projectId:guid}")]
        public async Task<IActionResult> Upsert(Guid projectId, [FromBody] WidgetSettings dto)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project is null) return BadRequest($"Unknown projectId: {projectId}");

            var s = await _context.WidgetSettings.FirstOrDefaultAsync(ws => ws.ProjectId == projectId);

            if (s is null)
            {
                s = new WidgetSettings
                {
                    ProjectId = projectId,
                    ChatHeaderText = dto.ChatHeaderText,
                    InactiveMessage = dto.InactiveMessage,
                    PrimaryColor = dto.PrimaryColor,
                    SecondaryColor = dto.SecondaryColor,
                    UseGradient = dto.UseGradient,
                    CornerRadius = dto.CornerRadius,
                    ScriptTag = dto.ScriptTag
                };
                _context.WidgetSettings.Add(s);
            }
            else
            {
                s.ChatHeaderText = dto.ChatHeaderText;
                s.InactiveMessage = dto.InactiveMessage;
                s.PrimaryColor = dto.PrimaryColor;
                s.SecondaryColor = dto.SecondaryColor;
                s.UseGradient = dto.UseGradient;
                s.CornerRadius = dto.CornerRadius;
                s.ScriptTag = dto.ScriptTag;
            }

            await _context.SaveChangesAsync();
            return Ok(Map(s)); // return DTO only (no navs)
        }
    }

    public record WidgetSettingsDto(
    int Id,
    Guid ProjectId,
    string? ChatHeaderText,
    string? InactiveMessage,
    string PrimaryColor,
    string? SecondaryColor,
    bool UseGradient,
    int CornerRadius,
    string? ScriptTag
);

}
