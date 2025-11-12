using LedgerService.DTOs.Ledger;

namespace LedgerService.Services.Contract
{
    public interface ILedgerService
    {
        Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(Guid userId);
        Task<IEnumerable<BeneficiaryDto>> GetBeneficiariesByUserIdAsync(Guid userId);
        Task<BeneficiaryDto> CreateBeneficiaryAsync(Guid userId, CreateBeneficiaryDto newBeneficiary);
        Task DeleteBeneficiaryAsync(Guid userId, Guid beneficiaryId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId);
        Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionDto newTransaction);
        Task<AccountDto> CreateAccountAsync(Guid userId, CreateAccountDto newAccount);
    }
}
