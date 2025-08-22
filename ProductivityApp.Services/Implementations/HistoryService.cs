using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.Services.Interfaces;

namespace ProductivityApp.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Habit>> GetHabitsAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.Habits.Query().Where(h => h.UserId == userId);
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.JournalEntries.Query().Where(j => j.UserId == userId).OrderByDescending(j => j.Date);
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId)
        {
            return await _unitOfWork.JournalEntries.GetByIdAsync(entryId);
        }
    }
}
