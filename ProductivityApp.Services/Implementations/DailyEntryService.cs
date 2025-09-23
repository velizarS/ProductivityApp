using Microsoft.EntityFrameworkCore;
using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Common.Enums;
using ProductivityApp.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityApp.Services
{
    public class DailyEntryService : IDailyEntryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DailyEntryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<DailyEntry> GetDailyEntryByDateAsync(string userId, DateTime date)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date.Date == date.Date);
        }

        public async Task<DailyEntry> GetDailyEntryByIdAsync(string userId, Guid dailyEntryId)
        {
            return await _unitOfWork.DailyEntries.Query()
                .Include(d => d.HabitCompletions)
                .Include(d => d.Tasks)
                .Include(d => d.JournalEntries)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Id == dailyEntryId);
        }

        public async Task<HabitCompletion> AddHabitCompletionAsync(string userId, Guid habitId, bool isCompleted = true)
        {
            var dailyEntry = await GetOrCreateDailyEntryAsync(userId);

            var habitCompletion = new HabitCompletion
            {
                Id = Guid.NewGuid(),
                HabitId = habitId,
                DailyEntryId = dailyEntry.Id,
                Date = DateTime.Now,
                IsCompleted = isCompleted
            };

            await _unitOfWork.HabitCompletions.AddAsync(habitCompletion);
            await _unitOfWork.CompleteAsync();

            return habitCompletion;
        }

        public async Task<JournalEntry> AddJournalEntryAsync(string userId, MoodType mood, string note)
        {
            var dailyEntry = await GetOrCreateDailyEntryAsync(userId);

            var journalEntry = new JournalEntry
            {
                Id = Guid.NewGuid(),
                DailyEntryId = dailyEntry.Id,
                UserId = userId,
                Mood = mood,
                Note = note
            };

            await _unitOfWork.JournalEntries.AddAsync(journalEntry);
            await _unitOfWork.CompleteAsync();

            return journalEntry;
        }

        public async Task<TaskM> AddOrCompleteTaskAsync(string userId, Guid taskId, string? completionNote = null)
        {
            var dailyEntry = await GetOrCreateDailyEntryAsync(userId);

            var task = await _unitOfWork.TaskMs.Query()
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
                throw new Exception("Task not found.");

            if (task.DailyEntryId != dailyEntry.Id)
                task.DailyEntryId = dailyEntry.Id;

            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;
            if (!string.IsNullOrEmpty(completionNote))
                task.CompletionNote = completionNote;

            _unitOfWork.TaskMs.Update(task);
            await _unitOfWork.CompleteAsync();

            return task;
        }
    }
}
