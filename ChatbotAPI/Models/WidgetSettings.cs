using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class WidgetSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        [MaxLength(100)]
        public string? ChatHeaderText { get; set; }

        [MaxLength(500)]
        public string? InactiveMessage { get; set; }

        [MaxLength(20)]
        public required string PrimaryColor { get; set; }

        [MaxLength(20)]
        public string? SecondaryColor { get; set; }

        public bool UseGradient { get; set; } = false;
        public int CornerRadius { get; set; }
        public string? ScriptTag { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
