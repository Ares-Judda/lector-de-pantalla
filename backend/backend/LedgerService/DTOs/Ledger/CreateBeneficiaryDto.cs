using System.ComponentModel.DataAnnotations;

namespace LedgerService.DTOs.Ledger
{
    public class CreateBeneficiaryDto
    {
        [Required]
        public string Name { get; set; }
        public string? Alias { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string BankName { get; set; }
    }
}
