using ProductivityApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Interfaces
{
    public interface IHistoryService
    {
       
        Task<IEnumerable<DailyEntry>> GetDailyEntriesAsync(string userId, int pageNumber = 1, int pageSize = 10);

        Task<DailyEntry?> GetDailyEntryByIdAsync(Guid dailyEntryId);

        Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(string userId, int pageNumber = 1, int pageSize = 10);

        Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId);

        Task<IEnumerable<HabitCompletion>> GetHabitCompletionsAsync(string userId, int pageNumber = 1, int pageSize = 10);
    }
}
