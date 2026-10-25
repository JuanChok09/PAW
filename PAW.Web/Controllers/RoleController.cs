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

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result =
                await _roleService.GetRolesAsync();

            var roles = result.ToList();

            int totalItems = roles.Count;

            int totalPages =
                (int)Math.Ceiling((double)totalItems / pageSize);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (page < 1)
            {
                page = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var pagedRoles = roles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedRoles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role =
                await _roleService.GetRoleAsync(id);

            ViewData["DetailsTitle"] = "Role Details";

            return PartialView(
                "~/Views/Shared/_DetailsPartial.cshtml",
                role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role =
                await _roleService.GetRoleAsync(id);

            ViewData["EditTitle"] = "Edit Role";
            ViewData["FormId"] = "editRoleForm";
            ViewData["SaveFunction"] = "saveRoleEdit";

            ViewData["HiddenProperties"] =
                new string[]
                {
                    "RoleId"
                };

            ViewData["EditableProperties"] =
                new string[]
                {
                    "RoleName"
                };

            return PartialView(
                "~/Views/Shared/_EditPartial.cshtml",
                role);
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