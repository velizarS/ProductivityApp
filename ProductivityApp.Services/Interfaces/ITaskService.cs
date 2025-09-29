using ProductivityApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskM>> GetTasksAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<TaskM?> GetTaskByIdAsync(Guid taskId);
        Task<TaskM> AddTaskAsync(string userId, string title, string? description, DateTime dueDate);
        Task<TaskM> CompleteTaskAsync(string userId, Guid taskId, string? completionNote = null);
        Task DeleteTaskAsync(Guid taskId);
    }
}
