using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllAsync();
        Task<ItemDTO> GetByPublicIdAsync(Guid publicId);
        Task<List<ItemDTO>> GetByCategoryAsync(Guid categoryId);
        Task<bool> CreateAsync(ItemDTO model);
        Task<bool> UpdateAsync(ItemDTO model);
        Task<bool> DeleteAsync(Guid publicId);
    }
}
