using System.ComponentModel.DataAnnotations;

namespace BackendHackathon.DTOs.Profile
{
    public class UpdateConsentRequestDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public bool Granted { get; set; }
    }
}
