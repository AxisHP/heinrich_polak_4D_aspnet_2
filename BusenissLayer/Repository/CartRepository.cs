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
    public class CartRepository : BaseRepository<CartItemEntity>, ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CartItemEntity>> GetByUserAsync(Guid userPublicId)
        {
            return await _context.CartItems
                .Include(c => c.Item)
                .Where(c => c.UserPublicId == userPublicId)
                .ToListAsync();
        }

        public async Task<CartItemEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId)
        {
            return await _context.CartItems
                .Include(c => c.Item)
                .FirstOrDefaultAsync(c => c.UserPublicId == userPublicId && c.ItemPublicId == itemPublicId);
        }
    }
}
