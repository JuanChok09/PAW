using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result =
                await _taskService.GetTasksAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task =
                await _taskService.GetTaskAsync(id);

            return PartialView("_DetailsPartial", task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task =
                await _taskService.GetTaskAsync(id);

            return PartialView("_EditPartial", task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskDTO task)
        {
            task.LastModified = DateTime.Now;

            await _taskService.SaveTaskAsync(task);

            return Json(new { success = true });
        }

        public IActionResult Create()
        {
            return PartialView(
                "_CreatePartial",
                new TaskDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskDTO task)
        {
            task.Id = 0;
            task.CreatedAt = DateTime.Now;
            task.LastModified = DateTime.Now;

            await _taskService.SaveTaskAsync(task);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);

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