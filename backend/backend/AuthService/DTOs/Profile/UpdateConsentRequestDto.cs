using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Profile
{
    public class UpdateConsentRequestDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public bool Granted { get; set; }
    }
}
