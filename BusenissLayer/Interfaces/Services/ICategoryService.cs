using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByPublicIdAsync(Guid publicId);
        Task<bool> CreateAsync(CategoryDTO model);
        Task<bool> UpdateAsync(CategoryDTO model);
        Task<bool> DeleteAsync(Guid publicId);
    }
}
