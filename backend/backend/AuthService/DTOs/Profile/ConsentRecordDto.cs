namespace AuthService.DTOs.Profile
{
    public class ConsentRecordDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public bool Granted { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
