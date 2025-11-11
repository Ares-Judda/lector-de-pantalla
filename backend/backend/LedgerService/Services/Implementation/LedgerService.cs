using LedgerService.Data.Models;
using LedgerService.DTOs.Ledger;
using LedgerService.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Services.Implementation
{
    public class LedgerService : ILedgerService
    {
        private readonly LedgerDbContext _context;

        public LedgerService(LedgerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(Guid userId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId) 
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    AccountNumber = a.AccountNumber,
                    AccountType = a.AccountType,
                    Balance = a.Balance,
                    Currency = a.Currency
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BeneficiaryDto>> GetBeneficiariesByUserIdAsync(Guid userId)
        {
            return await _context.Beneficiaries
                .AsNoTracking()
                .Where(b => b.UserId == userId) 
                .Select(b => new BeneficiaryDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Alias = b.Alias,
                    AccountNumber = b.AccountNumber,
                    BankName = b.BankName,
                    IsFavorite = b.IsFavorite
                })
                .ToListAsync();
        }

        public async Task<BeneficiaryDto> CreateBeneficiaryAsync(Guid userId, CreateBeneficiaryDto newBeneficiary)
        {
            var existing = await _context.Beneficiaries.FirstOrDefaultAsync(b =>
                b.UserId == userId && b.AccountNumber == newBeneficiary.AccountNumber);

            if (existing != null)
            {
                throw new Exception("Este beneficiario ya existe.");
            }

            var beneficiary = new Beneficiary
            {
                UserId = userId, 
                Name = newBeneficiary.Name,
                Alias = newBeneficiary.Alias,
                AccountNumber = newBeneficiary.AccountNumber,
                BankName = newBeneficiary.BankName,
            };

            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();

            return new BeneficiaryDto
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Alias = beneficiary.Alias,
                AccountNumber = beneficiary.AccountNumber,
                BankName = beneficiary.BankName,
                IsFavorite = beneficiary.IsFavorite
            };
        }

        public async Task DeleteBeneficiaryAsync(Guid userId, Guid beneficiaryId)
        {
            var beneficiary = await _context.Beneficiaries
                .FirstOrDefaultAsync(b => b.Id == beneficiaryId && b.UserId == userId);

            if (beneficiary == null)
            {
                throw new Exception("Beneficiario no encontrado o no pertenece al usuario.");
            }

            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(t => t.FromAccount) 
                .Include(t => t.ToBeneficiary)
                .Where(t => t.FromAccount.UserId == userId) 
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    FromAccountId = t.FromAccountId,
                    ToBeneficiaryId = t.ToBeneficiaryId,
                    BeneficiaryName = t.ToBeneficiary.Name, 
                    Amount = t.Amount,
                    Status = t.Status,
                    SpeiReference = t.SpeiReference,
                    CreatedAt = t.CreatedAt
                })
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionDto newTransaction)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == newTransaction.FromAccountId && a.UserId == userId);

            if (account == null)
            {
                throw new Exception("La cuenta de origen no existe o no pertenece al usuario.");
            }

            var beneficiary = await _context.Beneficiaries
                .FirstOrDefaultAsync(b => b.Id == newTransaction.ToBeneficiaryId && b.UserId == userId);

            if (beneficiary == null)
            {
                throw new Exception("El beneficiario no existe o no pertenece al usuario.");
            }

            if (account.Balance < newTransaction.Amount)
            {
                throw new Exception("Saldo insuficiente.");
            }

            // 4. (Opcional) Aquí iría la llamada al 'SecurityAlerts' si el monto es alto

            var transaction = new Transaction
            {
                FromAccountId = account.Id,
                ToBeneficiaryId = beneficiary.Id,
                Amount = newTransaction.Amount,
                Status = "COMPLETED", 
                SpeiReference = Guid.NewGuid().ToString().Substring(0, 10) 
            };

            account.Balance -= newTransaction.Amount;
            account.LastUpdated = DateTime.UtcNow;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                FromAccountId = transaction.FromAccountId,
                ToBeneficiaryId = transaction.ToBeneficiaryId,
                BeneficiaryName = beneficiary.Name,
                Amount = transaction.Amount,
                Status = transaction.Status,
                SpeiReference = transaction.SpeiReference,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}
