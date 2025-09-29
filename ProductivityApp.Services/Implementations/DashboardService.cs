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
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DailyEntry>> GetUserDailyEntriesAsync(string userId)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Where(d => d.UserId == userId)
                .Include(d => d.HabitCompletions)
                    .ThenInclude(hc => hc.Habit)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .OrderByDescending(d => d.Date)
                .ToListAsync();
        }

        public async Task<DailyEntry?> GetTodayDailyEntryAsync(string userId)
        {
            var today = DateTime.Today;
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                    .ThenInclude(hc => hc.Habit)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == today);
        }
    }
}
