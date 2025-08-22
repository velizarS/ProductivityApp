using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class JournalEntry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Entry Date")]
        [Comment("The date of the journal entry")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Mood")]
        [Comment("The mood of the user for this entry")]
        public MoodType Mood { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Note")]
        [Comment("Optional note for the journal entry")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "User")]
        [Comment("User who owns this journal entry")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public ICollection<TaskM> Tasks { get; set; } = new List<TaskM>();

    }
}
