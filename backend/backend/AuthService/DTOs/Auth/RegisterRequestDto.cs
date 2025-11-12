using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "El alias es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El alias debe tener entre 3 y 50 caracteres")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? PhoneNumber { get; set; }

        public string? PreferredLanguage { get; set; } = "es";
    }
}
