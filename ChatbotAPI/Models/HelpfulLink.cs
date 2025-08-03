using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatbotAPI.Models
{
    public class HelpfulLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TrainingDataId { get; set; }

        [ForeignKey("TrainingDataId")]
        public required TrainingData TrainingData { get; set; }
   
        public string? Url { get; set; }
    }
}
