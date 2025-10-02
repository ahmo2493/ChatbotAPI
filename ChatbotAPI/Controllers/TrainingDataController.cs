using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingDataController : ControllerBase
    {
        private readonly ChatbotDbContext _context;
        public TrainingDataController(ChatbotDbContext ctx) => _context = ctx;

        [HttpGet("{projectId:guid}")]
        public async Task<IActionResult> Get(Guid projectId)
        {
            var td = await _context.TrainingData
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ProjectId == projectId);

            return td is null ? NotFound() : Ok(td);
        }

        [HttpPost("{projectId:guid}")]
        public async Task<IActionResult> Upsert(Guid projectId, [FromBody] TrainingData dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project is null) return BadRequest($"Unknown projectId: {projectId}");

            var existing = await _context.TrainingData.FirstOrDefaultAsync(t => t.ProjectId == projectId);

            if (existing is null)
            {
                var entity = new TrainingData
                {
                    ProjectId = projectId,
                    BusinessName = dto.BusinessName,
                    WebsiteUrl = dto.WebsiteUrl,
                    ContactLink = dto.ContactLink,
                    TrainingText = dto.TrainingText
                };
                _context.TrainingData.Add(entity);
            }
            else
            {
                existing.BusinessName = dto.BusinessName;
                existing.WebsiteUrl = dto.WebsiteUrl;
                existing.ContactLink = dto.ContactLink;
                existing.TrainingText = dto.TrainingText;
            }

            // Auto-rename the project when BusinessName is provided
            if (!string.IsNullOrWhiteSpace(dto.BusinessName))
            {
                var newName = dto.BusinessName.Trim();
                if (!string.Equals(project.Name, newName, StringComparison.Ordinal))
                {
                    project.Name = newName;            // overwrite "Draft" (or any value) with BusinessName
                    project.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            var result = await _context.TrainingData.AsNoTracking()
                .FirstAsync(t => t.ProjectId == projectId);

            return Ok(result);
        }
    }
}
