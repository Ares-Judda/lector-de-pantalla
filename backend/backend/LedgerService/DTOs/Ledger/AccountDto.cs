namespace LedgerService.DTOs.Ledger
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
    }
}
