using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Profile
{
    public class UpdateUserProfileDto
    {
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? PhoneNumber { get; set; }

        public string? PreferredLanguage { get; set; }
    }
}
