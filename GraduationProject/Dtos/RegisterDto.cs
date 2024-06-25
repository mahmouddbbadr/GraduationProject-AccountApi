using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string? Email { get; set; }
    }
}
