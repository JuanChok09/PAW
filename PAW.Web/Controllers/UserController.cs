using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _userService.GetUsersAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user =
                await _userService.GetUserAsync(id);

            return PartialView("_DetailsPartial", user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user =
                await _userService.GetUserAsync(id);

            return PartialView("_EditPartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            user.LastModified = DateTime.Now;

            await _userService.SaveUserAsync(user);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new UserDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO user)
        {
            user.UserId = 0;
            user.CreatedAt = DateTime.Now;
            user.LastModified = DateTime.Now;

            await _userService.SaveUserAsync(user);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);

            return Json(new { success = true });
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });
        }
    }
}