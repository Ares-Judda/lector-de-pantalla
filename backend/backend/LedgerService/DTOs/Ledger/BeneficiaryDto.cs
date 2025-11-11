namespace LedgerService.DTOs.Ledger
{
    public class BeneficiaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Alias { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
