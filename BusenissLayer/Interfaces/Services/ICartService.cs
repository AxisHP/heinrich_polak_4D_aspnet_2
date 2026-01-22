using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface ICartService
    {
        Task<List<CartItemDTO>> GetByUserAsync(Guid userPublicId);
        Task<CartOperationResult> AddAsync(Guid userPublicId, Guid itemPublicId, int quantity);
        Task<CartOperationResult> UpdateQuantityAsync(Guid userPublicId, Guid itemPublicId, int quantity);
        Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId);
        Task<bool> ClearCartAsync(Guid userPublicId);
        Task<decimal> GetCartTotalAsync(Guid userPublicId);
        Task<int> GetCartCountAsync(Guid userPublicId);
    }
}
