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
        private readonly IItemRepository _itemRepository;

        public CartService(ICartRepository cartRepository, IItemRepository itemRepository)
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
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

        public async Task<CartOperationResult> AddAsync(Guid userPublicId, Guid itemPublicId, int quantity)
        {
            if (quantity <= 0)
                return CartOperationResult.FailureResult("Quantity must be greater than 0.");
            
            if (quantity > 1000)
                return CartOperationResult.FailureResult("Cannot add more than 1000 items of a single product. Maximum allowed quantity is 1000.");

            var item = await _itemRepository.GetByPublicIdAsync(itemPublicId);
            if (item == null)
                return CartOperationResult.FailureResult("Item not found.");

            var existing = await _cartRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            
            if (existing != null)
            {
                var newQuantity = existing.Quantity + quantity;
                
                if (newQuantity > 1000)
                    return CartOperationResult.FailureResult($"Cannot add {quantity} items. You already have {existing.Quantity} in cart. Maximum allowed quantity per item is 1000.");
                
                if (newQuantity > item.StockQuantity)
                    return CartOperationResult.FailureResult($"Insufficient stock. Only {item.StockQuantity} items available, but you're trying to add {newQuantity} total.");
                
                existing.Quantity = newQuantity;
                _cartRepository.Update(existing);
            }
            else
            {
                if (quantity > item.StockQuantity)
                    return CartOperationResult.FailureResult($"Insufficient stock. Only {item.StockQuantity} items available.");
                
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
            return CartOperationResult.SuccessResult();
        }

        public async Task<CartOperationResult> UpdateQuantityAsync(Guid userPublicId, Guid itemPublicId, int quantity)
        {
            var entity = await _cartRepository.GetByUserAndItemAsync(userPublicId, itemPublicId);
            if (entity == null) 
                return CartOperationResult.FailureResult("Cart item not found.");

            if (quantity <= 0)
            {
                _cartRepository.Delete(entity);
            }
            else
            {
                if (quantity > 1000)
                    return CartOperationResult.FailureResult("Cannot set quantity to more than 1000 items. Maximum allowed quantity per item is 1000.");
                
                var item = await _itemRepository.GetByPublicIdAsync(itemPublicId);
                if (item == null)
                    return CartOperationResult.FailureResult("Item not found.");
                
                if (quantity > item.StockQuantity)
                    return CartOperationResult.FailureResult($"Insufficient stock. Only {item.StockQuantity} items available.");
                
                entity.Quantity = quantity;
                _cartRepository.Update(entity);
            }

            await _cartRepository.SaveChangesAsync();
            return CartOperationResult.SuccessResult();
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
