using System.ComponentModel.DataAnnotations;

namespace OpenFinanceService.DTOs.OpenFinance
{
    public class CreateConnectionDto 
    {
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string Scopes { get; set; }
        [Required]
        public string AuthToken { get; set; }
    }
}
