using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class TrainingData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }   // optional nav; we use FK

        [MaxLength(200)]
        public string? BusinessName { get; set; }

        [MaxLength(400)]
        public string? WebsiteUrl { get; set; }

        public string? ContactLink { get; set; }
        public string? TrainingText { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
