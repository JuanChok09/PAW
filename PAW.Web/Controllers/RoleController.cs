using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleService roleService,
            ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetRolesAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role =
                await _roleService.GetRoleAsync(id);

            return PartialView("_DetailsPartial", role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role =
                await _roleService.GetRoleAsync(id);

            return PartialView("_EditPartial", role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleDTO role)
        {
            await _roleService.SaveRoleAsync(role);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new RoleDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO role)
        {
            role.RoleId = 0;

            await _roleService.SaveRoleAsync(role);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteRoleAsync(id);

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