using ProductivityApp.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Web.ViewModels.Journal
{
    public class JournalEntryCreateViewModel
    {
        [Required]
        public MoodType Mood { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
