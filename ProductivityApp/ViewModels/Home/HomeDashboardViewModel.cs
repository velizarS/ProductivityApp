using ProductivityApp.Web.ViewModels.DailyEntries;
using ProductivityApp.Web.ViewModels.Habits;
using ProductivityApp.Web.ViewModels.Journal;
using ProductivityApp.Web.ViewModels.Tasks;
using System.Collections.Generic;

namespace ProductivityApp.Web.ViewModels.Home
{
    public class HomeDashboardViewModel
    {
        public List<DailyEntryListViewModel> DailyEntries { get; set; } = new();
        public List<TaskListViewModel> Tasks { get; set; } = new();
        public List<HabitListViewModel> Habits { get; set; } = new();
        public List<JournalEntryListViewModel> JournalEntries { get; set; } = new();
    }
}
