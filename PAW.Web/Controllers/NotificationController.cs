using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _notificationService.GetNotificationsAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification =
                await _notificationService.GetNotificationAsync(id);

            return PartialView("_DetailsPartial", notification);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var notification =
                await _notificationService.GetNotificationAsync(id);

            return PartialView("_EditPartial", notification);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NotificationDTO notification)
        {
            await _notificationService.SaveNotificationAsync(notification);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new NotificationDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            notification.Id = 0;
            notification.CreatedAt = DateTime.Now;

            await _notificationService.SaveNotificationAsync(notification);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);

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