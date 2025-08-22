using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ProductivityApp.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();

        public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
    }
}
