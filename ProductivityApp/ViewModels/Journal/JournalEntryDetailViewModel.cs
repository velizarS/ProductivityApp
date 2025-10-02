using ProductivityApp.Common.Enums;
using System;

namespace ProductivityApp.Web.ViewModels.Journal
{
    public class JournalEntryDetailViewModel
    {
        public Guid Id { get; set; }
        public MoodType Mood { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
    }
}
