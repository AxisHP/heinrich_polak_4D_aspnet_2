using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;
using Common.Enums;

namespace BusinessLayer.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllAsync();
        Task<List<OrderDTO>> GetByUserAsync(Guid userPublicId);
        Task<OrderDTO> GetByPublicIdAsync(Guid publicId);
        Task<bool> CreateFromCartAsync(Guid userPublicId);
        Task<bool> UpdateStatusAsync(Guid publicId, OrderStatus status);
        Task<bool> CancelAsync(Guid publicId);
    }
}
