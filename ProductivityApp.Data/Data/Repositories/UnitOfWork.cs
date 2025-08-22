using ProductivityApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.Data.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Habit> Habits { get; private set; }
        public IRepository<HabitCompletion> HabitCompletions { get; private set; }
        public IRepository<JournalEntry> JournalEntries { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Habits = new EfRepository<Habit>(_context);
            HabitCompletions = new EfRepository<HabitCompletion>(_context);
            JournalEntries = new EfRepository<JournalEntry>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}