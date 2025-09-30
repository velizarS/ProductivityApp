using Microsoft.EntityFrameworkCore;
using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProductivityApp.Services.Implementations
{
    public class HabitsService : IHabitsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDailyEntryService _dailyEntryService;

        public HabitsService(IUnitOfWork unitOfWork, IDailyEntryService dailyEntryService)
        {
            _unitOfWork = unitOfWork;
            _dailyEntryService = dailyEntryService;
        }

        public async Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.Habits.Query()
                        .Where(h => h.UserId == userId)
                        .Include(h => h.Completions);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Habit?> GetHabitByIdAsync(Guid id)
        {
            return await _unitOfWork.Habits.Query()
                .Include(h => h.Completions)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task CreateHabitAsync(Habit habit)
        {
            await _unitOfWork.Habits.AddAsync(habit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateHabitAsync(Habit habit)
        {
            _unitOfWork.Habits.Update(habit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteHabitAsync(Guid id)
        {
            var habit = await _unitOfWork.Habits.GetByIdAsync(id);
            if (habit == null) return;

            habit.IsDeleted = true;
            _unitOfWork.Habits.Update(habit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task MarkHabitCompletedAsync(Guid habitId, DateTime date)
        {
            var habit = await _unitOfWork.Habits.GetByIdAsync(habitId);
            if (habit == null)
                throw new Exception("Habit not found.");

            // Вземаме или създаваме дневния запис за този потребител
            var dailyEntry = await _dailyEntryService.GetOrCreateDailyEntryAsync(habit.UserId, date);

            // Създаваме нов запис за изпълнение на навика
            var habitCompletion = new HabitCompletion
            {
                Id = Guid.NewGuid(),
                HabitId = habitId,
                DailyEntryId = dailyEntry.Id,
                Date = date,
                IsCompleted = true
            };

            await _unitOfWork.HabitCompletions.AddAsync(habitCompletion);
            await _unitOfWork.CompleteAsync();
        }
    }
}
