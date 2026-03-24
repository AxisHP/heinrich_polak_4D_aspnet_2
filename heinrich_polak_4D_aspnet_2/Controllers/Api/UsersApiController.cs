using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Common.Enums;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users.Select(ToUserResponse));
        }

        [HttpGet("{publicId:guid}")]
        public async Task<IActionResult> GetUser(Guid publicId)
        {
            var user = await _userService.GetByPublicIdAsync(publicId);
            if (user == null)
                return NotFound();

            return Ok(ToUserResponse(user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApiCreateUserRequest request)
        {
            var userDto = new UserDTO
            {
                PublicId = Guid.NewGuid(),
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Password = request.Password,
                Role = request.Role
            };

            var created = await _userService.CreateAsync(userDto);
            if (!created)
                return Conflict(new { message = "User with this email already exists." });

            return CreatedAtAction(nameof(GetUser), new { publicId = userDto.PublicId }, ToUserResponse(userDto));
        }

        [HttpPut("{publicId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid publicId, [FromBody] ApiUpdateUserRequest request)
        {
            var updateDto = new UserDTO
            {
                PublicId = publicId,
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Role = request.Role,
                Password = string.Empty
            };

            var updated = await _userService.UpdateAsync(updateDto);
            if (!updated)
                return NotFound();

            return Ok(ToUserResponse(updateDto));
        }

        [HttpDelete("{publicId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid publicId)
        {
            var deleted = await _userService.DeleteAsync(publicId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("delete-range")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<Guid> userPublicIds)
        {
            var deleted = await _userService.DeleteRangeAsync(userPublicIds);
            if (!deleted)
                return BadRequest(new { message = "At least one user id must be provided." });

            return NoContent();
        }

        [HttpPost("{publicId:guid}/reset-password")]
        public async Task<IActionResult> ResetPassword(Guid publicId, [FromBody] ApiResetPasswordRequest request)
        {
            var reset = await _userService.ResetPasswordAsync(publicId, request.NewPassword);
            if (!reset)
                return NotFound();

            return Ok(new { message = "Password reset successfully." });
        }

        private static object ToUserResponse(UserDTO user)
        {
            return new
            {
                user.PublicId,
                user.Name,
                user.LastName,
                user.Email,
                user.DateOfBirth,
                user.PhoneNumber,
                user.Address,
                user.Role
            };
        }
    }
}
