using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(
            IUserRoleService userRoleService,
            ILogger<UserRoleController> logger)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _userRoleService.GetUserRolesAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var userRole =
                await _userRoleService.GetUserRoleAsync(id);

            return PartialView("_DetailsPartial", userRole);
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var userRole =
                await _userRoleService.GetUserRoleAsync(id);

            return PartialView("_EditPartial", userRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleDTO userRole)
        {
            await _userRoleService.SaveUserRoleAsync(userRole);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new UserRoleDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleDTO userRole)
        {
            await _userRoleService.SaveUserRoleAsync(userRole);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(decimal id)
        {
            await _userRoleService.DeleteUserRoleAsync(id);

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