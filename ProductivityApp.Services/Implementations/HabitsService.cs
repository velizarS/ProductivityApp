using Microsoft.EntityFrameworkCore;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Data.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        .Where(h => h.UserId == userId && !h.IsDeleted)
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
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);
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
            habit.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Habits.Update(habit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<int> GetHabitsCountAsync(string userId)
        {
            return await _unitOfWork.Habits.Query()
                .CountAsync(h => h.UserId == userId && !h.IsDeleted);
        }


        public async Task<(bool isCompleted, int completedCount, int totalCount)> ToggleHabitAsync(Guid habitId, DateTime date)
        {
            var habit = await _unitOfWork.Habits.GetByIdAsync(habitId);
            if (habit == null)
                throw new Exception("Habit not found.");

            var dailyEntry = await _dailyEntryService.GetOrCreateDailyEntryAsync(habit.UserId, date.Date);

            var completion = await _unitOfWork.HabitCompletions.Query()
                .FirstOrDefaultAsync(h => h.HabitId == habitId && h.DailyEntryId == dailyEntry.Id);

            if (completion == null)
            {
                completion = new HabitCompletion
                {
                    Id = Guid.NewGuid(),
                    HabitId = habitId,
                    DailyEntryId = dailyEntry.Id,
                    Date = date.Date,
                    IsCompleted = true
                };
                await _unitOfWork.HabitCompletions.AddAsync(completion);
            }
            else
            {
                completion.IsCompleted = !completion.IsCompleted;
                _unitOfWork.HabitCompletions.Update(completion);
            }

            await _unitOfWork.CompleteAsync();

            var userId = habit.UserId;
            var totalCount = await GetHabitsCountAsync(userId);
            var completedCount = await _unitOfWork.HabitCompletions.Query()
                .CountAsync(h => h.Habit.UserId == userId
                              && h.Date.Date == date.Date
                              && h.IsCompleted
                              && !h.IsDeleted);

            return (completion.IsCompleted, completedCount, totalCount);
        }
    }
}

