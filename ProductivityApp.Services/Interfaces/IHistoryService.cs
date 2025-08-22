using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<IEnumerable<Habit>> GetHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId);
    }


}
