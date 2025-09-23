using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class DailyEntry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ICollection<HabitCompletion> HabitCompletions { get; set; } = new List<HabitCompletion>();
        public ICollection<TaskM> Tasks { get; set; } = new List<TaskM>();
        public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
