using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByPublicIdAsync(Guid publicId);
        Task<bool> CreateAsync(UserDTO model);
        Task<bool> UpdateAsync(UserDTO model);
        Task<bool> DeleteAsync(Guid publicId);
        Task<bool> DeleteRangeAsync(List<Guid> userPublicIds);
        Task<bool> ResetPasswordAsync(Guid userPublicId, string newPassword);
        Task<UserDTO> AuthenticateAsync(string email, string password);
    }
}
