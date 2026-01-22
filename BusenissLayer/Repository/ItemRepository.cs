using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using UserApp.DataLayer;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Repository
{
    public class ItemRepository : BaseRepository<ItemEntity>, IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ItemEntity>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Items
                .Include(i => i.Category)
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<ItemEntity>> GetAllWithCategoryAsync()
        {
            return await _context.Items
                .Include(i => i.Category)
                .ToListAsync();
        }

        public async Task<ItemEntity> GetByPublicIdWithCategoryAsync(Guid publicId)
        {
            return await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.PublicId == publicId);
        }
    }
}
