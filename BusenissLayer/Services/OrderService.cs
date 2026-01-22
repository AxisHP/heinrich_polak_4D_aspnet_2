using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Common.Enums;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IItemRepository _itemRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IItemRepository itemRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task<List<OrderDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllWithItemsAsync();
            var result = new List<OrderDTO>();

            foreach (var order in orders)
            {
                result.Add(MapToDTO(order));
            }

            return result;
        }

        public async Task<List<OrderDTO>> GetByUserAsync(Guid userPublicId)
        {
            var orders = await _orderRepository.GetByUserAsync(userPublicId);
            var result = new List<OrderDTO>();

            foreach (var order in orders)
            {
                result.Add(MapToDTO(order));
            }

            return result;
        }

        public async Task<OrderDTO> GetByPublicIdAsync(Guid publicId)
        {
            var order = await _orderRepository.GetByPublicIdWithItemsAsync(publicId);
            if (order == null) return null;

            return MapToDTO(order);
        }

        public async Task<bool> CreateFromCartAsync(Guid userPublicId)
        {
            var cartItems = await _cartRepository.GetByUserAsync(userPublicId);
            if (cartItems == null || !cartItems.Any()) return false;

            foreach (var cartItem in cartItems)
            {
                var item = await _itemRepository.GetByPublicIdAsync(cartItem.ItemPublicId);
                if (item == null || item.StockQuantity < cartItem.Quantity)
                {
                    return false;
                }
            }

            var order = new OrderEntity
            {
                PublicId = Guid.NewGuid(),
                UserPublicId = userPublicId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Payed,
                TotalAmount = cartItems.Sum(c => c.Item.Price * c.Quantity),
                Items = new List<OrderItem>()
            };

            foreach (var cartItem in cartItems)
            {
                order.Items.Add(new OrderItem
                {
                    PublicId = Guid.NewGuid(),
                    OrderPublicId = order.PublicId,
                    ItemPublicId = cartItem.ItemPublicId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Item.Price
                });

                var item = await _itemRepository.GetByPublicIdAsync(cartItem.ItemPublicId);
                if (item != null)
                {
                    item.StockQuantity -= cartItem.Quantity;
                    _itemRepository.Update(item);
                }
            }

            await _orderRepository.CreateAsync(order);
            
            foreach (var cartItem in cartItems)
            {
                _cartRepository.Delete(cartItem);
            }

            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid publicId, OrderStatus status)
        {
            var order = await _orderRepository.GetByPublicIdAsync(publicId);
            if (order == null) return false;

            order.Status = status;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(Guid publicId)
        {
            var order = await _orderRepository.GetByPublicIdWithItemsAsync(publicId);
            if (order == null) return false;

            foreach (var orderItem in order.Items)
            {
                var item = await _itemRepository.GetByPublicIdAsync(orderItem.ItemPublicId);
                if (item != null)
                {
                    item.StockQuantity += orderItem.Quantity;
                    _itemRepository.Update(item);
                }
            }

            order.Status = OrderStatus.Canceled;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        private OrderDTO MapToDTO(OrderEntity order)
        {
            return new OrderDTO
            {
                PublicId = order.PublicId,
                UserPublicId = order.UserPublicId,
                UserName = order.User?.Name ?? "",
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.Items?.Select(oi => new OrderItemDTO
                {
                    PublicId = oi.PublicId,
                    ItemPublicId = oi.ItemPublicId,
                    ItemName = oi.Item?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList() ?? new List<OrderItemDTO>()
            };
        }
    }
}
