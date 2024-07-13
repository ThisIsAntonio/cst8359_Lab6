using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    public class Prediction
    {
        public int PredictionId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public Question Question { get; set; }
    }

    public enum Question
    {
        Earth,
        Computer
    }
}

