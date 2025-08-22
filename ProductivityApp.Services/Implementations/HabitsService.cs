using Microsoft.EntityFrameworkCore;
using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using System.Linq;

namespace ProductivityApp.Services.Implementations
{
    public class HabitsService : IHabitsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HabitsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.Habits.Query()
                        .Where(h => h.UserId == userId);

            var pagedHabits = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return pagedHabits;
        }


        public async Task<Habit?> GetHabitByIdAsync(Guid id)
        {
            return await _unitOfWork.Habits.GetByIdAsync(id);
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
            var completion = new HabitCompletion { HabitId = habitId, Date = date, IsCompleted = true };
            await _unitOfWork.HabitCompletions.AddAsync(completion);
            await _unitOfWork.CompleteAsync();
        }
    }
}
