using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteService(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        public async Task<List<FavouriteDTO>> GetByUserAsync(Guid userPublicId)
        {
            var favourites = await _favouriteRepository.GetByUserAsync(userPublicId);
            var result = new List<FavouriteDTO>();

            foreach (var fav in favourites)
            {
                result.Add(new FavouriteDTO
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
            var existing = await _favouriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (existing != null) return false;

            var entity = new FavouriteEntity
            {
                PublicId = Guid.NewGuid(),
                UserPublicId = userPublicId,
                ItemPublicId = itemPublicId
            };

            await _favouriteRepository.CreateAsync(entity);
            await _favouriteRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId)
        {
            var entity = await _favouriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (entity == null) return false;

            _favouriteRepository.Delete(entity);
            await _favouriteRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsItemFavouriteAsync(Guid userPublicId, Guid itemPublicId)
        {
            var entity = await _favouriteRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            return entity != null;
        }
    }
}
