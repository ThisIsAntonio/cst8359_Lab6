using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    public class Prediction
    {
        public int PredictionId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FileName { get; set; }

        [Required]
        [DataType(DataType.Url)]
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

