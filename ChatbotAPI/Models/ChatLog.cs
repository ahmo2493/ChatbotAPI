using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class ChatLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public required Project Project { get; set; }

        public string? SessionId { get; set; }

        public string? UserMessage { get; set; }

        public string? BotResponse { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
