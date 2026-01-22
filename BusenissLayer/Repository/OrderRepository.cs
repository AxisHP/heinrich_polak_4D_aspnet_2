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
    public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderEntity>> GetByUserAsync(Guid userPublicId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Item)
                .Include(o => o.User)
                .Where(o => o.UserPublicId == userPublicId)
                .ToListAsync();
        }

        public async Task<OrderEntity> GetByPublicIdWithItemsAsync(Guid publicId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Item)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.PublicId == publicId);
        }

        public async Task<List<OrderEntity>> GetAllWithItemsAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Item)
                .Include(o => o.User)
                .ToListAsync();
        }
    }
}
