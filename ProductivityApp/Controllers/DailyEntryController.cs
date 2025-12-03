using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.DailyEntries;
using ProductivityApp.Web.ViewModels.DailyEntry;
using ProductivityApp.Web.ViewModels.Habits;
using ProductivityApp.Web.ViewModels.Journal;
using ProductivityApp.Web.ViewModels.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductivityApp.Web.Controllers
{
    [Authorize]
    public class DailyEntryController : Controller
    {
        private readonly IDailyEntryService _dailyEntryService;
        private readonly IMapper _mapper;

        public DailyEntryController(IDailyEntryService dailyEntryService, IMapper mapper)
        {
            _dailyEntryService = dailyEntryService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var entries = await _dailyEntryService.GetAllDailyEntriesAsync(userId);

            var model = _mapper.Map<List<DailyEntryListViewModel>>(entries);

            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var userId = GetUserId();
            var dailyEntry = await _dailyEntryService.GetDailyEntryByIdAsync(userId, id);

            if (dailyEntry == null) return NotFound();

            var habits = dailyEntry.HabitCompletions
                .Where(hc => hc.Habit != null)
                .Select(hc => new HabitListViewModel
                {
                    Id = hc.Habit.Id,
                    Name = hc.Habit.Name,
                    Description = hc.Habit.Description,
                    Frequency = hc.Habit.Frequency,
                    TotalCompletions = hc.Habit.Completions.Count,
                    IsCompletedToday = hc.IsCompleted,
                    IsDeleted = hc.Habit.IsDeleted
                }).ToList();

            var tasks = dailyEntry.Tasks
                .Select(t => new TaskListViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    IsDeleted = t.IsDeleted
                }).ToList();

            var journalEntries = dailyEntry.JournalEntries
                .Select(j => new JournalEntryListViewModel
                {
                    Id = j.Id,
                    Mood = j.Mood,
                    Note = j.Note,
                    Date = j.Date
                }).ToList();

            var model = new DailyEntryDetailViewModel
            {
                Id = dailyEntry.Id,
                Date = dailyEntry.Date,
                Habits = habits,
                Tasks = tasks,
                JournalEntries = journalEntries
            };

            return View(model);
        }
    }
}
