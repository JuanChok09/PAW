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

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var result =
                await _taskService.GetTasksAsync();

            var tasks = result.ToList();

            int totalItems = tasks.Count;

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

            var pagedTasks = tasks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedTasks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task =
                await _taskService.GetTaskAsync(id);

            ViewData["DetailsTitle"] = "Task Details";

            return PartialView(
                "~/Views/Shared/_DetailsPartial.cshtml",
                task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task =
                await _taskService.GetTaskAsync(id);

            ViewData["EditTitle"] = "Edit Task";
            ViewData["FormId"] = "editTaskForm";
            ViewData["SaveFunction"] = "saveTaskEdit";

            ViewData["HiddenProperties"] =
                new string[]
                {
                    "Id",
                    "CreatedAt"
                };

            ViewData["EditableProperties"] =
                new string[]
                {
                    "Name",
                    "Description",
                    "Status",
                    "DueDate",
                    "ModifiedBy"
                };

            return PartialView(
                "~/Views/Shared/_EditPartial.cshtml",
                task);
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