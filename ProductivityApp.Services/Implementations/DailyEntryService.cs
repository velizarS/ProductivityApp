using Microsoft.EntityFrameworkCore;
using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Implementations
{
    public class DailyEntryService : IDailyEntryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DailyEntryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DailyEntry>> GetAllDailyEntriesAsync(string userId)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .Where(d => d.UserId == userId && !d.IsDeleted)
                .OrderByDescending(d => d.Date)
                .ToListAsync();
        }


        public async Task<DailyEntry> GetOrCreateDailyEntryAsync(string userId, DateTime? date = null)
        {
            var targetDate = date?.Date ?? DateTime.Today;

            var dailyEntry = await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == targetDate);

            if (dailyEntry != null)
                return dailyEntry;

            dailyEntry = new DailyEntry
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = targetDate
            };

            await _unitOfWork.DailyEntries.AddAsync(dailyEntry);
            await _unitOfWork.CompleteAsync();

            return dailyEntry;
        }

        public async Task<DailyEntry?> GetDailyEntryByDateAsync(string userId, DateTime date)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == date.Date);
        }

        public async Task<DailyEntry?> GetDailyEntryByIdAsync(string userId, Guid dailyEntryId)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Id == dailyEntryId);
        }

        public async Task<IEnumerable<HabitCompletion>> GetHabitsForDayAsync(string userId, DateTime date)
        {
            var dailyEntry = await GetDailyEntryByDateAsync(userId, date);
            return dailyEntry?.HabitCompletions ?? Enumerable.Empty<HabitCompletion>();
        }

        public async Task<IEnumerable<TaskM>> GetTasksForDayAsync(string userId, DateTime date)
        {
            var dailyEntry = await GetDailyEntryByDateAsync(userId, date);
            return dailyEntry?.Tasks ?? Enumerable.Empty<TaskM>();
        }

        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesForDayAsync(string userId, DateTime date)
        {
            var dailyEntry = await GetDailyEntryByDateAsync(userId, date);
            return dailyEntry?.JournalEntries ?? Enumerable.Empty<JournalEntry>();
        }
    }
}
