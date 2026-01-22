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
    public class FavoriteRepository : BaseRepository<FavoriteEntity>, IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FavoriteEntity>> GetByUserAsync(Guid userPublicId)
        {
            return await _context.Favorites
                .Include(f => f.Item)
                .Where(f => f.UserPublicId == userPublicId)
                .ToListAsync();
        }

        public async Task<FavoriteEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserPublicId == userPublicId && f.ItemPublicId == itemPublicId);
        }
    }
}
