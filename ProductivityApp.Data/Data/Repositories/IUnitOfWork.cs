using ProductivityApp.Models.Models;

namespace ProductivityApp.Data.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Habit> Habits { get; }
        IRepository<HabitCompletion> HabitCompletions { get; }
        IRepository<JournalEntry> JournalEntries { get; }
        IRepository<TaskM> TaskMs { get; }

        Task<int> CompleteAsync();
    }
}