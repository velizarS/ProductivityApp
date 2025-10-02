using ProductivityApp.Common.Enums;
using System;

namespace ProductivityApp.Web.ViewModels.Habits
{
    public class HabitListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public FrequencyType Frequency { get; set; }
        public int TotalCompletions { get; set; }
        public bool IsCompletedToday { get; set; }
        public bool IsDeleted { get; set; }
    }
}
