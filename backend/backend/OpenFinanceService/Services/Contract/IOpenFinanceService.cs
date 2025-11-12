using OpenFinanceService.DTOs.OpenFinance;

namespace OpenFinanceService.Services.Contract
{
    public interface IOpenFinanceService
    {
        Task<IEnumerable<ConnectionDto>> GetConnectionsAsync(Guid userId);
        Task<ConnectionDto> CreateConnectionAsync(Guid userId, CreateConnectionDto request);
        Task DeleteConnectionAsync(Guid userId, Guid connectionId);

        Task<IEnumerable<ExternalProductDto>> GetExternalProductsAsync(Guid userId);
        Task<IEnumerable<ExternalProductDto>> SyncConnectionAsync(Guid userId, Guid connectionId);
    }
}
