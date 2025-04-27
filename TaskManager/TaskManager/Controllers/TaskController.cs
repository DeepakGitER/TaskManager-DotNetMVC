using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Dto;
using TaskManager.Enumerations;
using TaskManager.Interface;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILoginService _loginService;
        private readonly ICategoryService _categoryService;

        public TaskController(ITaskService taskService, ILoginService loginService, ICategoryService categoryService)
        {
            _taskService = taskService;
            _loginService = loginService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetTasksAsync();
            return View(tasks);
        }

        public async Task<IActionResult> Create()
        {
            var taskDto = await PopulateTaskDtoAsync(new TaskDto());
            ViewData["Title"] = "Create";
            return View(taskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskDto taskDto)
        {
            if (ModelState.IsValid)
            {
                await _taskService.CreateTaskAsync(taskDto);
                return RedirectToAction(nameof(Index));
            }

            taskDto = await PopulateTaskDtoAsync(taskDto);
            return View(taskDto);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            var taskDto = new TaskDto
            {
                TaskId = task.TaskId, 
                Title = task.Title,
                Description = task.Description,
                Priority = Enum.TryParse<Priority>(task.Priority, out var priority) ? priority : (Priority?)null,
                UserId = task.UserId,
                CategoryId = task.CategoryId
            };

            taskDto = await PopulateTaskDtoAsync(taskDto);
            ViewData["Title"] = "Edit Task";

            return View(taskDto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskDto taskDto)
        {
            if (ModelState.IsValid)
            {
                var task = await _taskService.GetTaskByIdAsync(taskDto.TaskId);
                if (task == null)
                    return NotFound();

                task.Title = taskDto.Title;
                task.Description = taskDto.Description;
                task.UserId = taskDto.UserId;
                task.Priority = taskDto.Priority.HasValue ? taskDto.Priority.Value.ToString() : null; 
                task.CategoryId = taskDto.CategoryId;
                task.UpdatedAt = DateTime.UtcNow;

                await _taskService.UpdateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            taskDto = await PopulateTaskDtoAsync(taskDto);
            return View(taskDto);
        }
        public async Task<IActionResult> Delete(int id)
        {

            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task<TaskDto> PopulateTaskDtoAsync(TaskDto taskDto)
        {
            var users = await _loginService.GetAllUsersAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            taskDto.Users = users?.Select(u => new SelectListItem { Value = u.UserId.ToString(), Text = u.Username }).ToList() ?? new List<SelectListItem>();
            taskDto.Categories = categories?.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList() ?? new List<SelectListItem>();
            taskDto.PriorityList = Enum.GetValues(typeof(Priority))
                .Cast<Priority>()
                .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
                .ToList();

            return taskDto;
        }

        [Authorize] 
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var userId = HttpContext.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var tasks = await _taskService.GetTasksByUserIdAsync(int.Parse(userId));
            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("tasks/user/{userId}")]
        public async Task<IActionResult> GetTasksByUser(int userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            if (tasks == null || tasks.Count == 0)
                return NotFound("No tasks found for this user.");

            return Ok(tasks);
        }


    }
}
