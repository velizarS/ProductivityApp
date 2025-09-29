using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DailyEntry>> GetDailyEntriesAsync(
            string userId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _unitOfWork.DailyEntries.Query()
                        .Where(d => d.UserId == userId)
                        .Include(d => d.HabitCompletions)
                        .Include(d => d.JournalEntries)
                        .Include(d => d.Tasks)
                        .OrderByDescending(d => d.Date);
             
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<DailyEntry?> GetDailyEntryByIdAsync(Guid dailyEntryId)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.JournalEntries)
                .Include(d => d.Tasks)
                .FirstOrDefaultAsync(d => d.Id == dailyEntryId);
        }

        public async Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(
            string userId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _unitOfWork.JournalEntries.Query()
                        .Where(j => j.UserId == userId)
                        .OrderByDescending(j => j.Date);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId)
        {
            return await _unitOfWork.JournalEntries.GetByIdAsync(entryId);
        }
       
        public async Task<IEnumerable<HabitCompletion>> GetHabitCompletionsAsync(
            string userId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _unitOfWork.HabitCompletions.Query()
                        .Where(h => h.Habit.UserId == userId)
                        .Include(h => h.Habit)
                        .Include(h => h.DailyEntry)
                        .OrderByDescending(h => h.Date);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
