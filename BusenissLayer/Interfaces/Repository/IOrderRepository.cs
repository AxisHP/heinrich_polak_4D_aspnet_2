using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        Task<List<OrderEntity>> GetByUserAsync(Guid userPublicId);
        Task<OrderEntity> GetByPublicIdWithItemsAsync(Guid publicId);
        Task<List<OrderEntity>> GetAllWithItemsAsync();
    }
}
