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
    public class FavouriteRepository : BaseRepository<FavouriteEntity>, IFavouriteRepository
    {
        private readonly AppDbContext _context;

        public FavouriteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FavouriteEntity>> GetByUserAsync(Guid userPublicId)
        {
            return await _context.Favourites
                .Include(f => f.Item)
                .Where(f => f.UserPublicId == userPublicId)
                .ToListAsync();
        }

        public async Task<FavouriteEntity> GetByUserAndItemAsync(Guid userPublicId, Guid itemPublicId)
        {
            return await _context.Favourites
                .FirstOrDefaultAsync(f => f.UserPublicId == userPublicId && f.ItemPublicId == itemPublicId);
        }
    }
}
