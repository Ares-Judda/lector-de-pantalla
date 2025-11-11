using LedgerService.DTOs.Ledger;
using LedgerService.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LedgerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class LedgerController : BaseController 
    {
        private readonly ILedgerService _ledgerService;

        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetMyAccounts()
        {
            var userId = GetUserIdFromToken();
            var accounts = await _ledgerService.GetAccountsByUserIdAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetMyBeneficiaries()
        {
            var userId = GetUserIdFromToken();
            var beneficiaries = await _ledgerService.GetBeneficiariesByUserIdAsync(userId);
            return Ok(beneficiaries);
        }

        [HttpPost("beneficiaries")]
        public async Task<IActionResult> CreateBeneficiary([FromBody] CreateBeneficiaryDto newBeneficiary)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            try
            {
                var beneficiary = await _ledgerService.CreateBeneficiaryAsync(userId, newBeneficiary);
                return CreatedAtAction(nameof(GetMyBeneficiaries), new { id = beneficiary.Id }, beneficiary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("beneficiaries/{id:guid}")]
        public async Task<IActionResult> DeleteBeneficiary(Guid id)
        {
            var userId = GetUserIdFromToken();
            try
            {
                await _ledgerService.DeleteBeneficiaryAsync(userId, id);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = GetUserIdFromToken();
            var transactions = await _ledgerService.GetTransactionsByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto newTransaction)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            try
            {
                var transaction = await _ledgerService.CreateTransactionAsync(userId, newTransaction);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
