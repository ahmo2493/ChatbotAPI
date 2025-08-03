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

        [ForeignKey("ProjectId")]
        public required Project Project { get; set; }

        // New fields for the various sources of training
        public string? TrainingText { get; set; } // from customText

        public ICollection<HelpfulLink> HelpfulLinks { get; set; } = new List<HelpfulLink>();

        public ICollection<PdfFile> PdfFiles { get; set; } = new List<PdfFile>();

        public string? ContactLink { get; set; }

        public string? BusinessCategory { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
