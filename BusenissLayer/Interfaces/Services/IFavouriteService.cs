using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface IFavouriteService
    {
        Task<List<FavouriteDTO>> GetByUserAsync(Guid userPublicId);
        Task<bool> AddAsync(Guid userPublicId, Guid itemPublicId);
        Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId);
        Task<bool> IsItemFavouriteAsync(Guid userPublicId, Guid itemPublicId);
    }
}
