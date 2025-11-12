using LedgerService.DTOs.Ledger;
using LedgerService.Services.Contract; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LedgerService.Controllers
{
    [ApiController]
    [Route("api/ledger")] 
    [Authorize]
    public class LedgerController : BaseController
    {
        private readonly ILedgerService _ledgerService;

        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserIdFromToken();
                var newAccount = await _ledgerService.CreateAccountAsync(userId, createAccountDto);

                return CreatedAtAction(nameof(GetAccounts), new { id = newAccount.Id }, newAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = GetUserIdFromToken();
            var accounts = await _ledgerService.GetAccountsByUserIdAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("beneficiaries")]
        public async Task<IActionResult> GetBeneficiaries()
        {
            var userId = GetUserIdFromToken();
            var beneficiaries = await _ledgerService.GetBeneficiariesByUserIdAsync(userId);
            return Ok(beneficiaries);
        }

        [HttpPost("beneficiaries")]
        public async Task<IActionResult> CreateBeneficiary([FromBody] CreateBeneficiaryDto createBeneficiaryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var newBeneficiary = await _ledgerService.CreateBeneficiaryAsync(userId, createBeneficiaryDto);
                return CreatedAtAction(nameof(GetBeneficiaries), new { id = newBeneficiary.Id }, newBeneficiary);
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
        public async Task<IActionResult> GetTransactions()
        {
            var userId = GetUserIdFromToken();
            var transactions = await _ledgerService.GetTransactionsByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var newTransaction = await _ledgerService.CreateTransactionAsync(userId, createTransactionDto);
                return Ok(newTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}