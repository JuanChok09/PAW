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

        public async Task<IActionResult> Index()
        {
            var result =
                await _supplierService.GetSuppliersAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier =
                await _supplierService.GetSupplierAsync(id);

            return PartialView("_DetailsPartial", supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier =
                await _supplierService.GetSupplierAsync(id);

            return PartialView("_EditPartial", supplier);
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