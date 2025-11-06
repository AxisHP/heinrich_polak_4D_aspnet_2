using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using heinrich_polak_4D_aspnet_2.Models;
using UserApp.DataLayer;
using UserApp.DataLayer.Entities;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            UserEntity user = new UserEntity();
            _logger = logger;
            _context = context;
        }

        public IActionResult userDetail(Guid userPublicid)
        {
            var user = _context.Users
                                .FirstOrDefault(u => u.PublicId == userPublicid);

            return View(user);
        }
        public IActionResult Users()

        {
            var userList = _context.Users.ToList();
            return View(userList);

        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var user = new UserEntity();

            return View(new CreateUserModel());
        }


        [HttpPost]
        public IActionResult CreateUser(CreateUserModel user)
        {
            var entity = new UserEntity()
            {
                Name = user.Username,
                Email = user.Email,
                PublicId = Guid.NewGuid()
            };

            _context.Users.Add(entity);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }


        [HttpGet]
        public IActionResult UpdateUser(Guid PublicId)
        {
            var model = new UpdateUserModel() { UserPublicId = PublicId };
            return View(model);
        }


        [HttpPost]
        public IActionResult UpdateUser(UpdateUserModel updateModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.PublicId == updateModel.UserPublicId);

            if (user != null)
            {
                user.Email = updateModel.Email;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid PublicId)
        {
            var model = new DeleteUserModel() { UserPublicId = PublicId };
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteUser(DeleteUserModel deleteModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.PublicId == deleteModel.UserPublicId);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Users");
        }

        public IActionResult Index()
        {
            var db = new AppDbContext();
            var users = db.Users.ToList();

            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
