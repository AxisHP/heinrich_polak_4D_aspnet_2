using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IFavoriteRepository : IBaseRepository<FavoriteEntity>
    {
        Task<List<FavoriteEntity>> GetByUserAsync(Guid userPublicId);
        Task<FavoriteEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId);
    }
}
