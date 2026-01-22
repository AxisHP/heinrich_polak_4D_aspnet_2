using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemDTO>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllWithCategoryAsync();
            var result = new List<ItemDTO>();

            foreach (var item in items)
            {
                result.Add(new ItemDTO
                {
                    PublicId = item.PublicId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category?.Name ?? ""
                });
            }

            return result;
        }

        public async Task<ItemDTO> GetByPublicIdAsync(Guid publicId)
        {
            var item = await _itemRepository.GetByPublicIdWithCategoryAsync(publicId);
            if (item == null) return null;

            return new ItemDTO
            {
                PublicId = item.PublicId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                StockQuantity = item.StockQuantity,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.Name ?? ""
            };
        }

        public async Task<List<ItemDTO>> GetByCategoryAsync(Guid categoryId)
        {
            var items = await _itemRepository.GetByCategoryAsync(categoryId);
            var result = new List<ItemDTO>();

            foreach (var item in items)
            {
                result.Add(new ItemDTO
                {
                    PublicId = item.PublicId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category?.Name ?? ""
                });
            }

            return result;
        }

        public async Task<bool> CreateAsync(ItemDTO model)
        {
            var entity = new ItemEntity
            {
                PublicId = model.PublicId == Guid.Empty ? Guid.NewGuid() : model.PublicId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId
            };

            await _itemRepository.CreateAsync(entity);
            await _itemRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ItemDTO model)
        {
            var entity = await _itemRepository.GetByPublicIdAsync(model.PublicId);
            if (entity == null) return false;

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.StockQuantity = model.StockQuantity;
            entity.CategoryId = model.CategoryId;

            _itemRepository.Update(entity);
            await _itemRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            var entity = await _itemRepository.GetByPublicIdAsync(publicId);
            if (entity == null) return false;

            _itemRepository.Delete(entity);
            await _itemRepository.SaveChangesAsync();
            return true;
        }
    }
}
