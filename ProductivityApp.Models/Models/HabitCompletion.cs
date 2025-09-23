using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class HabitCompletion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid HabitId { get; set; }
        public Habit Habit { get; set; }

        [Required]
        public Guid DailyEntryId { get; set; }
        public DailyEntry DailyEntry { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
