using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenFinanceService.DTOs.OpenFinance;
using OpenFinanceService.Services.Contract;

namespace OpenFinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class OpenFinanceController : BaseController
    {
        private readonly IOpenFinanceService _openFinanceService;

        public OpenFinanceController(IOpenFinanceService openFinanceService)
        {
            _openFinanceService = openFinanceService;
        }

        [HttpGet("connections")]
        public async Task<IActionResult> GetConnections()
        {
            var userId = GetUserIdFromToken();
            var connections = await _openFinanceService.GetConnectionsAsync(userId);
            return Ok(connections);
        }

        [HttpPost("connections")]
        public async Task<IActionResult> CreateConnection([FromBody] CreateConnectionDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            var connection = await _openFinanceService.CreateConnectionAsync(userId, request);

            return CreatedAtAction(nameof(GetConnections), new { id = connection.Id }, connection);
        }

        [HttpDelete("connections/{id:guid}")]
        public async Task<IActionResult> DeleteConnection(Guid id)
        {
            var userId = GetUserIdFromToken();
            try
            {
                await _openFinanceService.DeleteConnectionAsync(userId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetExternalProducts()
        {
            var userId = GetUserIdFromToken();
            var products = await _openFinanceService.GetExternalProductsAsync(userId);
            return Ok(products);
        }

        [HttpPost("sync/{connectionId:guid}")]
        public async Task<IActionResult> SyncConnection(Guid connectionId)
        {
            var userId = GetUserIdFromToken();
            try
            {
                var newProducts = await _openFinanceService.SyncConnectionAsync(userId, connectionId);
                return Ok(newProducts);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
