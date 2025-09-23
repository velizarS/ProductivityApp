using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Web.ViewModels.Habits
{
    public class HabitDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public FrequencyType Frequency { get; set; }
        public List<DateTime> Completions { get; set; }
        public int TotalCompletions { get; set; }

    }
}
