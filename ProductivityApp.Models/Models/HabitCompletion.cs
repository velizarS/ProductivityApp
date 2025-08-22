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
        [Display(Name = "Habit")]
        [Comment("The habit being completed")]
        public Guid HabitId { get; set; }
        public Habit Habit { get; set; }

        [Required]
        [Display(Name = "Completion Date")]
        [Comment("The date when the habit was completed")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Completed?")]
        [Comment("Indicates if the habit was completed on this date")]
        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
