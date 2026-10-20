using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserActionController : Controller
    {
        private readonly IUserActionService _userActionService;
        private readonly ILogger<UserActionController> _logger;

        public UserActionController(
            IUserActionService userActionService,
            ILogger<UserActionController> logger)
        {
            _userActionService = userActionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _userActionService.GetUserActionsAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(decimal id)
        {
            var userAction =
                await _userActionService.GetUserActionAsync(id);

            return PartialView("_DetailsPartial", userAction);
        }

        public async Task<IActionResult> Edit(decimal id)
        {
            var userAction =
                await _userActionService.GetUserActionAsync(id);

            return PartialView("_EditPartial", userAction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserActionDTO userAction)
        {
            await _userActionService.SaveUserActionAsync(userAction);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new UserActionDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserActionDTO userAction)
        {
            await _userActionService.SaveUserActionAsync(userAction);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(decimal id)
        {
            await _userActionService.DeleteUserActionAsync(id);

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