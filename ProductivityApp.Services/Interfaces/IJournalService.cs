using ProductivityApp.Models.Models;

namespace ProductivityApp.Services.Interfaces
{
    public interface IJournalService
    {
        Task<IEnumerable<JournalEntry>> GetAllEntriesAsync(string userId, int pageNumber , int pageSize);
        Task<JournalEntry?> GetEntryByIdAsync(Guid id);
        Task CreateEntryAsync(JournalEntry entry);
        Task UpdateEntryAsync(JournalEntry entry);
        Task DeleteEntryAsync(Guid id);

    }
}
