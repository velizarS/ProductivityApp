using System.Collections.Generic;

namespace ProductivityApp.Web.ViewModels.Journal
{
    public class JournalEntryListPageViewModel
    {
        public List<JournalEntryListViewModel> Entries { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
