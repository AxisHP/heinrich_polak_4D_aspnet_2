using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataLayer;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(UserDTO model)
        {
            var userEntity = new UserApp.DataLayer.Entities.UserEntity
            {
                PublicId = model.PublicId == Guid.Empty ? Guid.NewGuid() : model.PublicId,
                Name = model.Name,
                Email = model.Email
            };

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.PublicId == publicId);
            if (userEntity == null) return false;

            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var userList = await _context.Users.ToListAsync();
            var userDTOList = new List<UserDTO>();

            foreach (var user in userList)
            {
                userDTOList.Add(new UserDTO
                {
                    PublicId = user.PublicId,
                    Name = user.Name,
                    Email = user.Email
                });
            }

            return userDTOList;
        }

        public async Task<UserDTO> GetByPublicIdAsync(Guid userPublicId)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.PublicId == userPublicId);

            if (userEntity == null) return null;

            return new UserDTO
            {
                PublicId = userEntity.PublicId,
                Name = userEntity.Name,
                Email = userEntity.Email
            };
        }

        public async Task<bool> UpdateAsync(UserDTO model)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.PublicId == model.PublicId);
            if (userEntity == null) return false;
            
            userEntity.Name = model.Name;
            userEntity.Email = model.Email;
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
