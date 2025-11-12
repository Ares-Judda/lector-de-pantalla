using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El alias es obligatorio")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
