using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.Tasks;
using System.Security.Claims;

namespace ProductivityApp.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index(int page = 1)
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId, page, 10);
            var totalCount = await _taskService.GetTasksCountAsync(userId);

            var listModel = _mapper.Map<List<TaskListViewModel>>(tasks);
            var model = new TaskListPageViewModel
            {
                Tasks = listModel,
                CurrentPage = page,
                PageSize = 10,
                TotalCount = totalCount
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new TaskCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = GetUserId();
            await _taskService.AddTaskAsync(userId, model.Title, model.Description, model.DueDate);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            var model = _mapper.Map<TaskEditViewModel>(task);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = GetUserId();
            var existingTask = await _taskService.GetTaskByIdAsync(model.Id);
            if (existingTask == null)
                return NotFound();

            existingTask.Title = model.Title;
            existingTask.Description = model.Description;
            existingTask.DueDate = model.DueDate;
            existingTask.IsCompleted = model.IsCompleted;

            if (model.IsCompleted && !string.IsNullOrEmpty(model.CompletionNote))
            {
                await _taskService.CompleteTaskAsync(userId, model.Id, model.CompletionNote);
            }
            else
            {
                await _taskService.UpdateTaskAsync(existingTask);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsPartial(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            var model = _mapper.Map<TaskDetailViewModel>(task);
            return PartialView("_TaskDetailsPartial", model);
        }

        public async Task<IActionResult> DeletePartial(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            var model = _mapper.Map<TaskDetailViewModel>(task);
            return PartialView("_TaskDeletePartial", model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            var model = _mapper.Map<TaskDetailViewModel>(task);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TaskDetailsPartial", model);
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _taskService.DeleteTaskAsync(id);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(Guid id, string? note)
        {
            var userId = GetUserId();
            await _taskService.CompleteTaskAsync(userId, id, note);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
