using System.ComponentModel.DataAnnotations;

namespace LedgerService.DTOs.Ledger
{
    public class CreateAccountDto
    {
        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El número de cuenta debe ser válido")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de cuenta debe ser válido")]
        public string AccountType { get; set; } = "DEFAULT";
    }
}
