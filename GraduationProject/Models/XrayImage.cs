using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class XrayImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public string PredictionResult { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
