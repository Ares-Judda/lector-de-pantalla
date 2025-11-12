using Microsoft.EntityFrameworkCore;
using OpenFinanceService.Data.Models;
using OpenFinanceService.DTOs.OpenFinance;
using OpenFinanceService.Services.Contract;

namespace OpenFinanceService.Services.Implementation
{
    public class OpenFinanceServiceImplementation : IOpenFinanceService
    {
        private readonly OpenFinanceDbContext _context;

        public OpenFinanceServiceImplementation(OpenFinanceDbContext context)
        {
            _context = context;
        }

        public async Task<ConnectionDto> CreateConnectionAsync(Guid userId, CreateConnectionDto request)
        {
            var newConnection = new OpenFinanceConnection
            {
                UserId = userId,
                ProviderName = request.ProviderName,
                Scopes = request.Scopes,
                AuthToken = request.AuthToken,
                Status = "ACTIVE",
                LastSync = DateTime.UtcNow
            };

            _context.OpenFinanceConnections.Add(newConnection);
            await _context.SaveChangesAsync();

            return new ConnectionDto
            {
                Id = newConnection.Id,
                ProviderName = newConnection.ProviderName,
                Status = newConnection.Status,
                LastSync = newConnection.LastSync
            };
        }

        public async Task DeleteConnectionAsync(Guid userId, Guid connectionId)
        {
            var connection = await _context.OpenFinanceConnections
                .FirstOrDefaultAsync(c => c.Id == connectionId && c.UserId == userId);

            if (connection == null)
            {
                throw new Exception("Conexión no encontrada o no pertenece al usuario.");
            }

            _context.OpenFinanceConnections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConnectionDto>> GetConnectionsAsync(Guid userId)
        {
            return await _context.OpenFinanceConnections
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new ConnectionDto
                {
                    Id = c.Id,
                    ProviderName = c.ProviderName,
                    Status = c.Status,
                    LastSync = c.LastSync
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ExternalProductDto>> GetExternalProductsAsync(Guid userId)
        {
            var connectionIds = await _context.OpenFinanceConnections
                .Where(c => c.UserId == userId && c.Status == "ACTIVE")
                .Select(c => c.Id)
                .ToListAsync();

            return await _context.ExternalProducts
                .AsNoTracking()
                .Where(p => connectionIds.Contains(p.ConnectionId))
                .Select(p => MapToProductDto(p))
                .ToListAsync();
        }

        public async Task<IEnumerable<ExternalProductDto>> SyncConnectionAsync(Guid userId, Guid connectionId)
        {
            var connection = await _context.OpenFinanceConnections
                .FirstOrDefaultAsync(c => c.Id == connectionId && c.UserId == userId);

            if (connection == null)
            {
                throw new Exception("Conexión no encontrada o no pertenece al usuario.");
            }

            var oldProducts = _context.ExternalProducts.Where(p => p.ConnectionId == connectionId);
            _context.ExternalProducts.RemoveRange(oldProducts);

            var newProducts = new List<ExternalProduct>
            {
                new ExternalProduct
                {
                    ConnectionId = connectionId,
                    Provider = connection.ProviderName,
                    ProductType = "CREDIT_CARD",
                    Name = $"Tarjeta {connection.ProviderName} (Sincronizada)",
                    Balance = -(decimal)(new Random().Next(1000, 15000) + 0.50), 
                    Currency = "MXN",
                    NextPaymentAmount = 1500,
                    NextPaymentDate = DateOnly.FromDateTime(DateTime.Today.AddDays(15)),
                    LastSync = DateTime.UtcNow
                },
                new ExternalProduct
                {
                    ConnectionId = connectionId,
                    Provider = connection.ProviderName,
                    ProductType = "LOAN",
                    Name = "Préstamo Personal",
                    Balance = -(decimal)(new Random().Next(50000, 150000)),
                    Currency = "MXN",
                    NextPaymentAmount = 4500,
                    NextPaymentDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
                    LastSync = DateTime.UtcNow
                }
            };

            _context.ExternalProducts.AddRange(newProducts);

            connection.LastSync = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return newProducts.Select(p => MapToProductDto(p));
        }

        private ExternalProductDto MapToProductDto(ExternalProduct p)
        {
            return new ExternalProductDto
            {
                Id = p.Id,
                ConnectionId = p.ConnectionId,
                Provider = p.Provider,
                ProductType = p.ProductType,
                Name = p.Name,
                Balance = p.Balance,
                Currency = p.Currency,
                NextPaymentAmount = p.NextPaymentAmount,
                NextPaymentDate = p.NextPaymentDate,
                LastSync = p.LastSync
            };
        }
    }
}
