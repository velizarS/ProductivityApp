using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IHabitsService
    {
        Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<Habit?> GetHabitByIdAsync(Guid id);
        Task<int> GetHabitsCountAsync(string userId);
        Task CreateHabitAsync(Habit habit);
        Task UpdateHabitAsync(Habit habit);
        Task DeleteHabitAsync(Guid id);
        Task<(bool isCompleted, int completedCount, int totalCount)> ToggleHabitAsync(Guid habitId, DateTime date);
    }
}
