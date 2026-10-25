using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            ISupplierService supplierService,
            ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result =
                await _supplierService.GetSuppliersAsync();

            var suppliers = result.ToList();

            int totalItems = suppliers.Count;

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

            var pagedSuppliers = suppliers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedSuppliers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier =
                await _supplierService.GetSupplierAsync(id);

            ViewData["DetailsTitle"] = "Supplier Details";

            return PartialView(
                "~/Views/Shared/_DetailsPartial.cshtml",
                supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier =
                await _supplierService.GetSupplierAsync(id);

            ViewData["EditTitle"] = "Edit Supplier";
            ViewData["FormId"] = "editSupplierForm";
            ViewData["SaveFunction"] = "saveSupplierEdit";

            ViewData["HiddenProperties"] =
                new string[]
                {
                    "SupplierId"
                };

            ViewData["EditableProperties"] =
                new string[]
                {
                    "SupplierName",
                    "ContactName",
                    "ContactTitle",
                    "Phone",
                    "Address",
                    "City",
                    "Country",
                    "ModifiedBy"
                };

            return PartialView(
                "~/Views/Shared/_EditPartial.cshtml",
                supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierDTO supplier)
        {
            supplier.LastModified = DateTime.Now;

            await _supplierService.SaveSupplierAsync(supplier);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new SupplierDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierDTO supplier)
        {
            supplier.SupplierId = 0;
            supplier.LastModified = DateTime.Now;

            await _supplierService.SaveSupplierAsync(supplier);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _supplierService.DeleteSupplierAsync(id);

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