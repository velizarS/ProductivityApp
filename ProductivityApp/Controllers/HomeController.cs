using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.DailyEntries;
using ProductivityApp.Web.ViewModels.Home;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductivityApp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDailyEntryService _dailyEntryService;
        private readonly IMapper _mapper;

        public HomeController(IDailyEntryService dailyEntryService, IMapper mapper)
        {
            _dailyEntryService = dailyEntryService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var dailyEntries = await _dailyEntryService.GetAllDailyEntriesAsync(userId);

            var model = new HomeDashboardViewModel
            {
                DailyEntries = _mapper.Map<List<DailyEntryListViewModel>>(dailyEntries)
            };

            return View(model);
        }
    }
}
