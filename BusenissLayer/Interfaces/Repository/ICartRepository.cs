using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface ICartRepository : IBaseRepository<CartItemEntity>
    {
        Task<List<CartItemEntity>> GetByUserAsync(Guid userPublicId);
        Task<CartItemEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId);
    }
}
