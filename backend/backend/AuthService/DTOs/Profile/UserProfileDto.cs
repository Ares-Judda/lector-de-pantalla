namespace AuthService.DTOs.Profile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string PreferredLanguage { get; set; }
        public bool DemoMode { get; set; }
    }
}
