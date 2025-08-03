using ChatbotAPI.Models;
using ChatbotAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly OpenAIService _openAi;

        public ChatController(OpenAIService openAi)
        {
            _openAi = openAi;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatLog request)
        {
            if (string.IsNullOrWhiteSpace(request.UserMessage))
                return BadRequest("Message is required.");


            // 🚧 Optional: Load user/project data from database here
            // var user = _dbContext.Users.FirstOrDefault(u => u.Id == request.UserId);
            // var project = _dbContext.Projects.FirstOrDefault(p => p.Id == request.ProjectId);

            var botReply = await _openAi.GetResponseAsync(request.UserMessage);

            // Attach AI reply
            request.BotResponse = botReply;
            request.Timestamp = DateTime.Now;

            // 🚧 Optional: Save to DB or log file
            // _dbContext.ChatLogs.Add(request);
            // await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                response = botReply,
                userMessage = request.UserMessage,
                timestamp = request.Timestamp
            });
        }
    }
}
