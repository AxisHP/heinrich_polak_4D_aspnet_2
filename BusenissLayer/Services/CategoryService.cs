using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var result = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                result.Add(new CategoryDTO
                {
                    PublicId = category.PublicId,
                    Name = category.Name,
                    Description = category.Description
                });
            }

            return result;
        }

        public async Task<CategoryDTO> GetByPublicIdAsync(Guid publicId)
        {
            var category = await _categoryRepository.GetByPublicIdAsync(publicId);
            if (category == null) return null;

            return new CategoryDTO
            {
                PublicId = category.PublicId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> CreateAsync(CategoryDTO model)
        {
            var entity = new CategoryEntity
            {
                PublicId = model.PublicId == Guid.Empty ? Guid.NewGuid() : model.PublicId,
                Name = model.Name,
                Description = model.Description
            };

            await _categoryRepository.CreateAsync(entity);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CategoryDTO model)
        {
            var entity = await _categoryRepository.GetByPublicIdAsync(model.PublicId);
            if (entity == null) return false;

            entity.Name = model.Name;
            entity.Description = model.Description;

            _categoryRepository.Update(entity);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            var entity = await _categoryRepository.GetByPublicIdAsync(publicId);
            if (entity == null) return false;

            _categoryRepository.Delete(entity);
            await _categoryRepository.SaveChangesAsync();
            return true;
        }
    }
}
