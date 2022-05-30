using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}