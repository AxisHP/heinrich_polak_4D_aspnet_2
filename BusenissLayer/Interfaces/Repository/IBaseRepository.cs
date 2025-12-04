using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : UserApp.DataLayer.Entities.BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByPublicIdAsync(Guid publicId);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
