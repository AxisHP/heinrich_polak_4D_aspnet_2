using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using heinrich_polak_4D_aspnet_2.Models;
using UserApp.DataLayer;
using UserApp.DataLayer.Entities;
using BusinessLayer.Services;
using BusinessLayer.Interfaces.Services;
using Common.DTO;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> UserList()
        {
            var dtos = await _userService.GetAllAsync();
            var entities = dtos.Select(d => new UserEntity
            {
                PublicId = d.PublicId,
                Name = d.Name,
                LastName = d.LastName,
                Email = d.Email,
                DateOfBirth = d.DateOfBirth,
                PhoneNumber = d.PhoneNumber,
                Address = d.Address,
                Role = d.Role
            }).ToList();

            return View(entities);
        }

        public async Task<IActionResult> userDetail(Guid userPublicid)
        {
            var dto = await _userService.GetByPublicIdAsync(userPublicid);
            if (dto is null) return NotFound();

            var entity = new UserEntity
            {
                PublicId = dto.PublicId,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = dto.Role
            };

            return View(entity);
        }

        public async Task<IActionResult> Users()
        {
            var dtos = await _userService.GetAllAsync();
            return View(dtos);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new CreateUserModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel user)
        {
            var dto = new UserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = user.Role,
                PublicId = Guid.NewGuid()
            };

            await _userService.CreateAsync(dto);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(Guid PublicId)
        {
            var dto = await _userService.GetByPublicIdAsync(PublicId);
            if (dto == null)
            {
                return NotFound();
            }

            var model = new UpdateUserModel
            {
                UserPublicId = PublicId,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = dto.Role
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateModel)
        {
            var dto = new UserDTO
            {
                PublicId = updateModel.UserPublicId,
                Name = updateModel.Name,
                LastName = updateModel.LastName,
                Email = updateModel.Email,
                DateOfBirth = updateModel.DateOfBirth,
                PhoneNumber = updateModel.PhoneNumber,
                Address = updateModel.Address,
                Role = updateModel.Role
            };

            await _userService.UpdateAsync(dto);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid PublicId)
        {
            var model = new DeleteUserModel { UserPublicId = PublicId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteUserModel deleteModel)
        {
            await _userService.DeleteAsync(deleteModel.UserPublicId);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRangeUser(string selectedUserIds)
        {
            if (!string.IsNullOrEmpty(selectedUserIds))
            {
                var userIds = selectedUserIds.Split(',')
                    .Where(id => Guid.TryParse(id, out _))
                    .Select(Guid.Parse)
                    .ToList();
                
                if (userIds.Any())
                {
                    await _userService.DeleteRangeAsync(userIds);
                }
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _userService.GetAllAsync();
            var entities = dtos.Select(d => new UserEntity
            {
                PublicId = d.PublicId,
                Name = d.Name,
                LastName = d.LastName,
                Email = d.Email,
                DateOfBirth = d.DateOfBirth,
                PhoneNumber = d.PhoneNumber,
                Address = d.Address,
                Role = d.Role
            }).ToList();

            return View(entities);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
