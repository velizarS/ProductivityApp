using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.Habits;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductivityApp.Web.Controllers
{
    [Authorize]
    public class HabitsController : Controller
    {
        private readonly IHabitsService _habitsService;
        private readonly IMapper _mapper;

        public HabitsController(IHabitsService habitsService, IMapper mapper)
        {
            _habitsService = habitsService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index(int page = 1)
        {
            var userId = GetUserId();

            var habits = await _habitsService.GetAllHabitsAsync(userId, page, 10);

            var listModel = _mapper.Map<List<HabitListViewModel>>(habits);

            var totalCount = await _habitsService.GetHabitsCountAsync(userId);

            var model = new HabitListPageViewModel
            {
                Habits = listModel,
                CurrentPage = page,
                PageSize = 10,
                TotalCount = totalCount
            };

            return View(model);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HabitCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var habit = _mapper.Map<Habit>(model);
            habit.Id = Guid.NewGuid();
            habit.UserId = GetUserId();

            await _habitsService.CreateHabitAsync(habit);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var habit = await _habitsService.GetHabitByIdAsync(id);
            if (habit == null) return NotFound();

            var model = _mapper.Map<HabitEditViewModel>(habit);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HabitEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var habit = await _habitsService.GetHabitByIdAsync(model.Id);
            if (habit == null) return NotFound();

            _mapper.Map(model, habit);
            await _habitsService.UpdateHabitAsync(habit);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var habit = await _habitsService.GetHabitByIdAsync(id);
            if (habit == null) return NotFound();

            var model = _mapper.Map<HabitDetailViewModel>(habit);
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var habit = await _habitsService.GetHabitByIdAsync(id);
            if (habit == null) return NotFound();

            var model = _mapper.Map<HabitDetailViewModel>(habit);
            return View(model);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _habitsService.DeleteHabitAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MarkComplete(Guid id)
        {
            await _habitsService.MarkHabitCompletedAsync(id, DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
    }
}
