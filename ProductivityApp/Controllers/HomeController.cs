using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.DailyEntries;
using ProductivityApp.Web.ViewModels.Home;
using ProductivityApp.Web.ViewModels.Habits;
using ProductivityApp.Web.ViewModels.Tasks;
using ProductivityApp.Web.ViewModels.Journal;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace ProductivityApp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDailyEntryService _dailyEntryService;
        private readonly ITaskService _taskService;
        private readonly IHabitsService _habitService;
        private readonly IJournalService _journalService;
        private readonly IMapper _mapper;

        public HomeController(
            IDailyEntryService dailyEntryService,
            ITaskService taskService,
            IHabitsService habitService,
            IJournalService journalService,
            IMapper mapper)
        {
            _dailyEntryService = dailyEntryService;
            _taskService = taskService;
            _habitService = habitService;
            _journalService = journalService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var dailyEntries = await _dailyEntryService.GetAllDailyEntriesAsync(userId);
            var tasks = await _taskService.GetTasksAsync(userId);
            var habits = await _habitService.GetAllHabitsAsync(userId);
            var journals = await _journalService.GetAllEntriesAsync(userId, 1, 10);

            var model = new HomeDashboardViewModel
            {
                DailyEntries = _mapper.Map<List<DailyEntryListViewModel>>(dailyEntries),
                Tasks = _mapper.Map<List<TaskListViewModel>>(tasks),
                Habits = _mapper.Map<List<HabitListViewModel>>(habits),
                JournalEntries = _mapper.Map<List<JournalEntryListViewModel>>(journals)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(Guid taskId)
        {
            var userId = GetUserId();
            await _taskService.CompleteTaskAsync(userId, taskId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            await _taskService.DeleteTaskAsync(taskId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PostponeTask(Guid taskId, DateTime newDueDate)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null) return NotFound();

            task.DueDate = newDueDate;
            await _taskService.UpdateTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TaskDetails(Guid taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null) return NotFound();

            var model = _mapper.Map<TaskListViewModel>(task);
            return View(model);
        }
    }
}
