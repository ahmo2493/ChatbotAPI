using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class PdfFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TrainingDataId { get; set; }

        [ForeignKey("TrainingDataId")]
        public required TrainingData TrainingData { get; set; }

        public required string FileName { get; set; }

        public required byte[] FileContent { get; set; }
    }
}
