using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Models;
using ChatbotAPI.Data;

namespace ChatbotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingDataController : ControllerBase
    {
        private readonly ChatbotDbContext _context;

        public TrainingDataController(ChatbotDbContext context)
        {
            _context = context;
        }

        // POST: api/TrainingData
        [HttpPost]
        public async Task<IActionResult> CreateTrainingData([FromBody] TrainingData trainingData)
        {
            if (trainingData == null || trainingData.ProjectId == Guid.Empty)
            {
                return BadRequest("Invalid training data.");
            }

            // Attach the TrainingData to its project
            var project = await _context.Projects.FindAsync(trainingData.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }

            trainingData.Project = project;

            _context.TrainingData.Add(trainingData);
            await _context.SaveChangesAsync();

            return Ok(trainingData);
        }

        // GET: api/TrainingData/{projectId}
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetTrainingData(Guid projectId)
        {
            var trainingData = await _context.TrainingData
                .Include(td => td.HelpfulLinks)
                .Include(td => td.PdfFiles)
                .FirstOrDefaultAsync(td => td.ProjectId == projectId);

            if (trainingData == null)
            {
                return NotFound();
            }

            return Ok(trainingData);
        }
    }
}
