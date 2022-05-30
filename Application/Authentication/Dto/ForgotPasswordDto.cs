using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}