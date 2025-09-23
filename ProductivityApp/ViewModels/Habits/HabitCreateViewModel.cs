using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Web.ViewModels.Habits
{
    public class HabitCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Habit Name")]
        public string Name { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        [Comment("Optional description for the habit")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Frequency")]
        public FrequencyType Frequency { get; set; }
    }
}