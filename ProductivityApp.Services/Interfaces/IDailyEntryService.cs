using ProductivityApp.Models.Models;
using ProductivityApp.Common.Enums;
using System;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Interfaces
{
    public interface IDailyEntryService
    {
        Task<DailyEntry> GetOrCreateDailyEntryAsync(string userId, DateTime? date = null);
        Task<DailyEntry> GetDailyEntryByDateAsync(string userId, DateTime date);
        Task<DailyEntry> GetDailyEntryByIdAsync(string userId, Guid dailyEntryId);
        Task<HabitCompletion> AddHabitCompletionAsync(string userId, Guid habitId, bool isCompleted = true);
        Task<JournalEntry> AddJournalEntryAsync(string userId, MoodType mood, string note);
        Task<TaskM> AddOrCompleteTaskAsync(string userId, Guid taskId, string? completionNote = null);
    }

}
