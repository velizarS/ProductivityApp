using ProductivityApp.Web.ViewModels.Habits;
using ProductivityApp.Web.ViewModels.Journal;
using ProductivityApp.Web.ViewModels.Tasks;

namespace ProductivityApp.Web.ViewModels.DailyEntry
{
    public class DailyEntryDetailViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public List<HabitListViewModel> Habits { get; set; } = new();
        public List<TaskListViewModel> Tasks { get; set; } = new();
        public List<JournalEntryListViewModel> JournalEntries { get; set; } = new();
    }

}
