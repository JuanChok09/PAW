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

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result =
                await _inventoryService.GetInventoriesAsync();

            var inventories = result.ToList();

            int totalItems = inventories.Count;

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

            var pagedInventories = inventories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedInventories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryAsync(id);

            ViewData["DetailsTitle"] = "Inventory Details";

            return PartialView(
                "~/Views/Shared/_DetailsPartial.cshtml",
                inventory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory =
                await _inventoryService.GetInventoryAsync(id);

            ViewData["EditTitle"] = "Edit Inventory";
            ViewData["FormId"] = "editInventoryForm";
            ViewData["SaveFunction"] = "saveInventoryEdit";

            ViewData["HiddenProperties"] =
                new string[]
                {
                    "InventoryId",
                    "DateAdded"
                };

            ViewData["EditableProperties"] =
                new string[]
                {
                    "UnitPrice",
                    "UnitsInStock",
                    "ProductId",
                    "ModifiedBy"
                };

            return PartialView(
                "~/Views/Shared/_EditPartial.cshtml",
                inventory);
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