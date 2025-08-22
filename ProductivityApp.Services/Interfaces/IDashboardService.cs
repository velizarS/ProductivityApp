using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<Habit>> GetUserHabitsAsync(string userId);
        Task<IEnumerable<HabitCompletion>> GetUserHabitCompletionsAsync(string userId);
    }
}
