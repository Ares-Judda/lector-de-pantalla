namespace OpenFinanceService.DTOs.OpenFinance
{
    public class ExternalProductDto
    {
        public Guid Id { get; set; }
        public Guid ConnectionId { get; set; }
        public string Provider { get; set; }
        public string ProductType { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public decimal? NextPaymentAmount { get; set; }
        public DateOnly? NextPaymentDate { get; set; }
        public DateTime LastSync { get; set; }
    }
}
