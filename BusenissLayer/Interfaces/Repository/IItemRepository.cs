using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IItemRepository : IBaseRepository<ItemEntity>
    {
        Task<List<ItemEntity>> GetByCategoryAsync(Guid categoryId);
        Task<List<ItemEntity>> GetAllWithCategoryAsync();
        Task<ItemEntity> GetByPublicIdWithCategoryAsync(Guid publicId);
    }
}
