using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartItemDTO>> GetByUserAsync(Guid userPublicId)
        {
            var cartItems = await _cartRepository.GetByUserAsync(userPublicId);
            var result = new List<CartItemDTO>();

            foreach (var item in cartItems)
            {
                result.Add(new CartItemDTO
                {
                    PublicId = item.PublicId,
                    UserPublicId = item.UserPublicId,
                    ItemPublicId = item.ItemPublicId,
                    ItemName = item.Item?.Name ?? "",
                    ItemPrice = item.Item?.Price ?? 0,
                    Quantity = item.Quantity
                });
            }

            return result;
        }

        public async Task<bool> AddAsync(Guid userPublicId, Guid itemPublicId, int quantity)
        {
            var existing = await _cartRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            
            if (existing != null)
            {
                existing.Quantity += quantity;
                _cartRepository.Update(existing);
            }
            else
            {
                var entity = new CartItemEntity
                {
                    PublicId = Guid.NewGuid(),
                    UserPublicId = userPublicId,
                    ItemPublicId = itemPublicId,
                    Quantity = quantity
                };
                await _cartRepository.CreateAsync(entity);
            }

            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(Guid userPublicId, Guid itemPublicId, int quantity)
        {
            var entity = await _cartRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (entity == null) return false;

            if (quantity <= 0)
            {
                _cartRepository.Delete(entity);
            }
            else
            {
                entity.Quantity = quantity;
                _cartRepository.Update(entity);
            }

            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid userPublicId, Guid itemPublicId)
        {
            var entity = await _cartRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (entity == null) return false;

            _cartRepository.Delete(entity);
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(Guid userPublicId)
        {
            var cartItems = await _cartRepository.GetByUserAsync(userPublicId);
            
            foreach (var item in cartItems)
            {
                _cartRepository.Delete(item);
            }

            await _cartRepository.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetCartTotalAsync(Guid userPublicId)
        {
            var cartItems = await _cartRepository.GetByUserAsync(userPublicId);
            return cartItems.Sum(c => (c.Item?.Price ?? 0) * c.Quantity);
        }

        public async Task<int> GetCartCountAsync(Guid userPublicId)
        {
            var cartItems = await _cartRepository.GetByUserAsync(userPublicId);
            return cartItems.Sum(c => c.Quantity);
        }
    }
}
