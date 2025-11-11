using System.ComponentModel.DataAnnotations;

namespace LedgerService.DTOs.Ledger
{
    public class CreateTransactionDto
    {
        [Required]
        public Guid FromAccountId { get; set; }
        [Required]
        public Guid ToBeneficiaryId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public decimal Amount { get; set; }
    }
}
