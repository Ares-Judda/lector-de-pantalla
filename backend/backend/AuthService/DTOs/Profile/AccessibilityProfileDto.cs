using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.Profile
{
    public class AccessibilityProfileDto
    {
        [Required]
        public string Theme { get; set; } = "light";

        [Required]
        public bool ScreenReaderMode { get; set; } = false;

        [Required]
        [Range(0.5, 3.0, ErrorMessage = "La escala de la fuente debe estar entre 0.5 y 3.0")]
        public decimal FontScale { get; set; } = 1.0m;

        [Required]
        public string NudgingLevel { get; set; } = "medium";

        [Required]
        public bool VoiceFeedback { get; set; } = false;
    }
}
