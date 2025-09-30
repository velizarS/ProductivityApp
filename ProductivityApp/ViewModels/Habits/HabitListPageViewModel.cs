// HabitListPageViewModel.cs
using System.Collections.Generic;

namespace ProductivityApp.Web.ViewModels.Habits
{
    public class HabitListPageViewModel
    {
        public List<HabitListViewModel> Habits { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
