using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IDailyEntryService
    {
        Task<DailyEntry> GetOrCreateDailyEntryAsync(string userId, DateTime? date = null);
        Task<DailyEntry?> GetDailyEntryByDateAsync(string userId, DateTime date);
        Task<DailyEntry?> GetDailyEntryByIdAsync(string userId, Guid dailyEntryId);

        Task<IEnumerable<HabitCompletion>> GetHabitsForDayAsync(string userId, DateTime date);
        Task<IEnumerable<TaskM>> GetTasksForDayAsync(string userId, DateTime date);
        Task<IEnumerable<JournalEntry>> GetJournalEntriesForDayAsync(string userId, DateTime date);
    }
}
