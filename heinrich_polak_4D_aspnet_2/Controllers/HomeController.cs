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
                Email = d.Email
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
                Email = dto.Email
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
                Name = user.Username,
                Email = user.Email,
                PublicId = Guid.NewGuid()
            };

            await _userService.CreateAsync(dto);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public IActionResult UpdateUser(Guid PublicId)
        {
            var model = new UpdateUserModel { UserPublicId = PublicId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateModel)
        {
            // Get the current user data to preserve the Name field
            var currentUser = await _userService.GetByPublicIdAsync(updateModel.UserPublicId);
            if (currentUser == null)
            {
                return NotFound();
            }

            var dto = new UserDTO
            {
                PublicId = updateModel.UserPublicId,
                Name = currentUser.Name, // Preserve existing name
                Email = updateModel.Email
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

        public async Task<IActionResult> Index()
        {
            var dtos = await _userService.GetAllAsync();
            var entities = dtos.Select(d => new UserEntity
            {
                PublicId = d.PublicId,
                Name = d.Name,
                Email = d.Email
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
