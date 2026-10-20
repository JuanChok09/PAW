using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;
using System.Diagnostics;

namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result = await _productService.GetProductsAsync();

            var products = result.ToList();

            int totalItems = products.Count;

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

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductAsync(id);

            return PartialView("_DetailsPartial", product);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductAsync(id);

            return PartialView("_EditPartial", product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product)
        {
            product.ModifiedDate = DateTime.Now;

            await _productService.SaveProductAsync(product);

            return Json(new { success = true });
        }
        public IActionResult Create()
        {
            return PartialView("_CreatePartial", new ProductDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            product.ProductId = 0;
            product.Comments = string.Empty;
            product.CreatedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;

            await _productService.SaveProductAsync(product);

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);

            return Json(new { success = true });
        }
    }
}
