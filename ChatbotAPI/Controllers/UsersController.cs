using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ChatbotDbContext _context;
        public UsersController(ChatbotDbContext ctx) => _context = ctx;

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(email) && int.TryParse(idStr, out var uid))
            {
                var u = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == uid);
                email = u?.Email ?? email;
            }

            if (string.IsNullOrEmpty(email)) return Unauthorized();
            return Ok(new { Email = email });
        }
    }
}
