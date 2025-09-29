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
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDailyEntryService _dailyEntryService;

        public TaskService(IUnitOfWork unitOfWork, IDailyEntryService dailyEntryService)
        {
            _unitOfWork = unitOfWork;
            _dailyEntryService = dailyEntryService;
        }

        public async Task<IEnumerable<TaskM>> GetTasksAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _unitOfWork.TaskMs.Query()
                        .Where(t => t.UserId == userId)
                        .Include(t => t.DailyEntry)
                        .OrderByDescending(t => t.DueDate);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TaskM?> GetTaskByIdAsync(Guid taskId)
        {
            return await _unitOfWork.TaskMs.Query()
                .Include(t => t.DailyEntry)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<TaskM> AddTaskAsync(string userId, string title, string? description, DateTime dueDate)
        {
            var dailyEntry = await _dailyEntryService.GetOrCreateDailyEntryAsync(userId);

            var task = new TaskM
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DailyEntryId = dailyEntry.Id,
                Title = title,
                Description = description,
                DueDate = dueDate,
                IsCompleted = false
            };

            await _unitOfWork.TaskMs.AddAsync(task);
            await _unitOfWork.CompleteAsync();

            return task;
        }

        public async Task<TaskM> CompleteTaskAsync(string userId, Guid taskId, string? completionNote = null)
        {
            var task = _unitOfWork.TaskMs.Query().FirstOrDefault(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                throw new Exception("Task not found.");

            var dailyEntry = await _dailyEntryService.GetOrCreateDailyEntryAsync(userId);
            task.DailyEntryId = dailyEntry.Id;

            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;
            if (!string.IsNullOrEmpty(completionNote))
                task.CompletionNote = completionNote;

            _unitOfWork.TaskMs.Update(task);
            await _unitOfWork.CompleteAsync();

            return task;
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            var task = await _unitOfWork.TaskMs.GetByIdAsync(taskId);
            if (task == null) return;

            task.IsDeleted = true;
            _unitOfWork.TaskMs.Update(task);
            await _unitOfWork.CompleteAsync();
        }
    }
}
