using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        void DeleteRange(List<UserEntity> userEntities);
        Task<List<UserEntity>> GetAll();
        Task<UserEntity?> GetByEmailAsync(string email);
    }
}
