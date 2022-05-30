using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.Dto
{
    public class TwoStepAuthenticationDto
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Provider { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}