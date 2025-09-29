using ProductivityApp.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductivityApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<DailyEntry>> GetUserDailyEntriesAsync(string userId);
        Task<DailyEntry?> GetTodayDailyEntryAsync(string userId);
    }
}
