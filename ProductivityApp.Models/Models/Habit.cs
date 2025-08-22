using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class Habit
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Habit Name")]
        [Comment("Name of the habit")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Frequency")]
        [Comment("How often the habit should be completed")]
        public FrequencyType Frequency { get; set; }

        [Required]
        [Display(Name = "User")]
        [Comment("User who owns this habit")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<HabitCompletion> Completions { get; set; } = new List<HabitCompletion>();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
