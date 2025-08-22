using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.Services.Interfaces;

namespace ProductivityApp.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Habit>> GetUserHabitsAsync(string userId)
        {
            return await _unitOfWork.Habits.Query()
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitCompletion>> GetUserHabitCompletionsAsync(string userId)
        {
            var habits = await GetUserHabitsAsync(userId);
            var habitIds = habits.Select(h => h.Id).ToList();

            return await _unitOfWork.HabitCompletions.Query()
                .Where(c => habitIds.Contains(c.HabitId))
                .ToListAsync();
        }
    }
}
