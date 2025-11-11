namespace LedgerService.DTOs.Ledger
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToBeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string? SpeiReference { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
