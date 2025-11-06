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
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            throw new NotImplementedException();
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

        public async Task<UserDTO> GetByPublicIdAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(UserDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
