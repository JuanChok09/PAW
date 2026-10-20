using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(
            IComponentService componentService,
            ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result =
                await _componentService.GetComponentsAsync();

            var components = result.ToList();

            int totalItems = components.Count;

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

            var pagedComponents = components
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedComponents);
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var component =
                await _componentService.GetComponentAsync(id);

            return PartialView("_DetailsPartial", component);
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var component =
                await _componentService.GetComponentAsync(id);

            return PartialView("_EditPartial", component);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ComponentDTO component)
        {
            await _componentService.SaveComponentAsync(component);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new ComponentDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComponentDTO component)
        {
            component.Id = 0;

            await _componentService.SaveComponentAsync(component);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(decimal id)
        {
            await _componentService.DeleteComponentAsync(id);

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