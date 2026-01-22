using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDTO>> GetByUserAsync(Guid userPublicId);
        Task<bool> AddAsync(Guid userPublicId, Guid itemPublicId);
        Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId);
        Task<bool> IsItemFavoriteAsync(Guid userPublicId, Guid itemPublicId);
    }
}
