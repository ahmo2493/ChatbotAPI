using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }

        [MaxLength(120)]
        public string? Name { get; set; } = "Draft"; // default, will be overwritten by TrainingData.BusinessName

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 1:1
        public WidgetSettings? WidgetSettings { get; set; }
        public TrainingData? TrainingData { get; set; }
    }
}
