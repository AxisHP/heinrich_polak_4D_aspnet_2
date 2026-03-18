using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Common.Enums;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApiLoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(ToUserResponse(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ApiRegisterRequest request)
        {
            var dto = new UserDTO
            {
                PublicId = Guid.NewGuid(),
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Password = request.Password,
                Role = UserRole.Customer
            };

            var created = await _userService.CreateAsync(dto);
            if (!created)
                return Conflict(new { message = "User with this email already exists." });

            return Ok(ToUserResponse(dto));
        }

        [HttpGet("users/{publicId:guid}")]
        public async Task<IActionResult> GetUser(Guid publicId)
        {
            var user = await _userService.GetByPublicIdAsync(publicId);
            if (user == null)
                return NotFound();

            return Ok(ToUserResponse(user));
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