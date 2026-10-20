using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _inventoryService.GetInventoriesAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryAsync(id);

            return PartialView("_DetailsPartial", inventory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryAsync(id);

            return PartialView("_EditPartial", inventory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InventoryDTO inventory)
        {
            inventory.LastUpdated = DateTime.Now;

            await _inventoryService.SaveInventoryAsync(inventory);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new InventoryDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryDTO inventory)
        {
            inventory.InventoryId = 0;
            inventory.DateAdded = DateTime.Now;
            inventory.LastUpdated = DateTime.Now;

            await _inventoryService.SaveInventoryAsync(inventory);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);

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