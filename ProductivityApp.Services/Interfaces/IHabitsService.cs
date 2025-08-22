using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IHabitsService
    {
        Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<Habit?> GetHabitByIdAsync(Guid id);
        Task CreateHabitAsync(Habit habit);
        Task UpdateHabitAsync(Habit habit);
        Task DeleteHabitAsync(Guid id);
        Task MarkHabitCompletedAsync(Guid habitId, DateTime date);
    }
}
