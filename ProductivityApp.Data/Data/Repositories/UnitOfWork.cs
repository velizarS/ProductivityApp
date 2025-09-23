using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Data.Data;
using ProductivityApp.Models.Models;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<Habit> Habits { get; private set; }
    public IRepository<HabitCompletion> HabitCompletions { get; private set; }
    public IRepository<JournalEntry> JournalEntries { get; private set; }
    public IRepository<TaskM> TaskMs { get; private set; }
    public IRepository<DailyEntry> DailyEntries { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Habits = new EfRepository<Habit>(_context);
        HabitCompletions = new EfRepository<HabitCompletion>(_context);
        JournalEntries = new EfRepository<JournalEntry>(_context);
        TaskMs = new EfRepository<TaskM>(_context);
        DailyEntries = new EfRepository<DailyEntry>(_context);
    }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
