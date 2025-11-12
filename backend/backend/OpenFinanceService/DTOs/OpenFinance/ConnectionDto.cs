namespace OpenFinanceService.DTOs.OpenFinance
{
    public class ConnectionDto
    {
        public Guid Id { get; set; }
        public string ProviderName { get; set; }
        public string Status { get; set; }
        public DateTime LastSync { get; set; }
    }
}
