using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<FavoriteDTO>> GetByUserAsync(Guid userPublicId)
        {
            var favorites = await _favoriteRepository.GetByUserAsync(userPublicId);
            var result = new List<FavoriteDTO>();

            foreach (var fav in favorites)
            {
                result.Add(new FavoriteDTO
                {
                    PublicId = fav.PublicId,
                    UserPublicId = fav.UserPublicId,
                    ItemPublicId = fav.ItemPublicId,
                    ItemName = fav.Item?.Name ?? "",
                    ItemPrice = fav.Item?.Price ?? 0
                });
            }

            return result;
        }

        public async Task<bool> AddAsync(Guid userPublicId, Guid itemPublicId)
        {
            var existing = await _favoriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (existing != null) return false;

            var entity = new FavoriteEntity
            {
                PublicId = Guid.NewGuid(),
                UserPublicId = userPublicId,
                ItemPublicId = itemPublicId
            };

            await _favoriteRepository.CreateAsync(entity);
            await _favoriteRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId)
        {
            var entity = await _favoriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (entity == null) return false;

            _favoriteRepository.Delete(entity);
            await _favoriteRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsItemFavoriteAsync(Guid userPublicId, Guid itemPublicId)
        {
            var entity = await _favoriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            return entity != null;
        }
    }
}
