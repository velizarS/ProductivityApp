using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Heplers;
using ProductivityApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Implementations
{
    public class JournalService : IJournalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDailyEntryService _dailyEntryService;
        private readonly IAesGcmEncryptionHelper _encryptionHelper;

        public JournalService(
            IUnitOfWork unitOfWork,
            IDailyEntryService dailyEntryService,
            IAesGcmEncryptionHelper encryptionHelper)
        {
            _unitOfWork = unitOfWork;
            _dailyEntryService = dailyEntryService;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<IEnumerable<JournalEntry>> GetAllEntriesAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.JournalEntries.Query()
                        .Where(j => j.UserId == userId)
                        .OrderByDescending(j => j.Date);

            var pagedEntries = await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            foreach (var entry in pagedEntries)
            {
                if (!string.IsNullOrEmpty(entry.Note))
                {
                    entry.Note = _encryptionHelper.Decrypt(entry.Note, entry.UserId);
                }
            }

            return pagedEntries;
        }

        public async Task<JournalEntry?> GetEntryByIdAsync(Guid id)
        {
            var entry = await _unitOfWork.JournalEntries.GetByIdAsync(id);
            if (entry != null && !string.IsNullOrEmpty(entry.Note))
            {
                entry.Note = _encryptionHelper.Decrypt(entry.Note, entry.UserId);
            }
            return entry;
        }

        public async Task CreateEntryAsync(JournalEntry entry)
        {
            if (!string.IsNullOrEmpty(entry.Note))
            {
                entry.Note = _encryptionHelper.Encrypt(entry.Note, entry.UserId);
            }

            await _dailyEntryService.AddJournalEntryAsync(
                userId: entry.UserId,
                mood: entry.Mood,
                note: entry.Note
            );
        }

        public async Task UpdateEntryAsync(JournalEntry entry)
        {
            if (!string.IsNullOrEmpty(entry.Note))
            {
                entry.Note = _encryptionHelper.Encrypt(entry.Note, entry.UserId);
            }

            _unitOfWork.JournalEntries.Update(entry);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteEntryAsync(Guid id)
        {
            var entry = await _unitOfWork.JournalEntries.GetByIdAsync(id);
            if (entry == null) return;

            entry.IsDeleted = true;
            _unitOfWork.JournalEntries.Update(entry);
            await _unitOfWork.CompleteAsync();
        }
    }
}
