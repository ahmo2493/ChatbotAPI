// using System.Text.Json.Serialization;  <-- add this using

using ChatbotAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ChatLog
{
    [Key] public int Id { get; set; }

    [Required] public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    [JsonIgnore]                  // keep nav off the wire
    public Project? Project { get; set; }  // <- nullable (was required)

    public string? SessionId { get; set; }
    public string? UserMessage { get; set; }
    public string? BotResponse { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
