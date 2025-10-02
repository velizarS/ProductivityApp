using System.Collections.Generic;

namespace ProductivityApp.Web.ViewModels.Tasks
{
    public class TaskListPageViewModel
    {
        public List<TaskListViewModel> Tasks { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}