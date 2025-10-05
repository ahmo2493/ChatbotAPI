// Controllers/ChatController.cs
using ChatbotAPI.Data;
using ChatbotAPI.Models;
using ChatbotAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // => /api/chat
    public class ChatController : ControllerBase
    {
        private readonly OpenAIService _openAi;
        private readonly ChatbotDbContext _db;

        public ChatController(OpenAIService openAi, ChatbotDbContext db)
        {
            _openAi = openAi;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatLog request)
        {
            if (request is null) return BadRequest("Body is required.");
            if (request.ProjectId == Guid.Empty) return BadRequest("projectId is required.");
            if (string.IsNullOrWhiteSpace(request.UserMessage)) return BadRequest("Message is required.");

            // Load business profile (if present)
            var td = await _db.TrainingData
                              .AsNoTracking()
                              .FirstOrDefaultAsync(t => t.ProjectId == request.ProjectId);
            var project = await _db.Projects
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(p => p.Id == request.ProjectId);

            var businessName = td?.BusinessName ?? project?.Name ?? "this business";
            var website = td?.WebsiteUrl ?? "your website";
            var contact = td?.ContactLink ?? td?.WebsiteUrl ?? website;

            // Optional notes to help the model (kept short)
            var notes = td?.TrainingText?.Trim();
            if (!string.IsNullOrEmpty(notes) && notes.Length > 2000)
                notes = notes.Substring(0, 2000);

            // Strong scope + refusal policy (no RAG yet)
            var system = $@"
You are the website assistant for ""{businessName}"" ({website}).
Your sole purpose is to help visitors with information about this specific business and its services.

Rules:
- ONLY answer using the 'Business Profile' below. Do not invent facts.
- If the question is unrelated to this business, refuse politely and steer the user back to relevant topics.
- If you don't know from the profile, say you don't have that info and give the contact link.
- Be brief, friendly, and helpful (2–6 short sentences).
";

            var profile = $@"Business Profile:
- Name: {businessName}
- Website: {website}
- Contact: {contact}
" + (string.IsNullOrWhiteSpace(notes) ? "" : ("- Notes:\n" + notes));

            var prompt = $"{system}\n\n{profile}\n\nUser: {request.UserMessage}\nAssistant:";

            // Call your existing OpenAI service
            var botReply = await _openAi.GetResponseAsync(prompt);

            // Log (useful now; invaluable later)
            var log = new ChatLog
            {
                ProjectId = request.ProjectId,
                SessionId = string.IsNullOrWhiteSpace(request.SessionId) ? "preview" : request.SessionId,
                UserMessage = request.UserMessage,
                BotResponse = botReply,
                Timestamp = DateTime.UtcNow
            };
            _db.ChatLogs.Add(log);
            await _db.SaveChangesAsync();

            return Ok(new { response = botReply, timestamp = log.Timestamp, id = log.Id });
        }
    }
}
