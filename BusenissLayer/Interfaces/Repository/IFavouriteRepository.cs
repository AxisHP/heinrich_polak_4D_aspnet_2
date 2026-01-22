using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IFavouriteRepository : IBaseRepository<FavouriteEntity>
    {
        Task<List<FavouriteEntity>> GetByUserAsync(Guid userPublicId);
        Task<FavouriteEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId);
    }
}
