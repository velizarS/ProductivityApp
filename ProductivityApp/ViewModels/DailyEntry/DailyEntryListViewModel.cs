using System;

namespace ProductivityApp.Web.ViewModels.DailyEntries
{
    public class DailyEntryListViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int HabitsCount { get; set; }
        public int TasksCount { get; set; }
        public int JournalCount { get; set; }
    }
}
