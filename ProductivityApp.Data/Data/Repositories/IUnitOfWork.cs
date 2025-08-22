using ProductivityApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityApp.Data.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Habit> Habits { get; }
        IRepository<HabitCompletion> HabitCompletions { get; }
        IRepository<JournalEntry> JournalEntries { get; }

        Task<int> CompleteAsync();
    }
}