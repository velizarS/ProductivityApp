using ProductivityApp.Models.Models;
using ProductivityApp.Data.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<Habit> Habits { get; }
    IRepository<HabitCompletion> HabitCompletions { get; }
    IRepository<JournalEntry> JournalEntries { get; }
    IRepository<TaskM> TaskMs { get; }
    IRepository<DailyEntry> DailyEntries { get; }  // <- добави това

    Task<int> CompleteAsync();
}
