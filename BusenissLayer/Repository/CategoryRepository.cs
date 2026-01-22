using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using UserApp.DataLayer;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Repository
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
