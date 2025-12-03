using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductivityApp.Services.Interfaces;
using ProductivityApp.Web.ViewModels.Journal;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductivityApp.Web.Controllers
{
    [Authorize]
    public class JournalController : Controller
    {
        private readonly IJournalService _journalService;
        private readonly IMapper _mapper;

        public JournalController(IJournalService journalService, IMapper mapper)
        {
            _journalService = journalService;
            _mapper = mapper;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Index(int page = 1)
        {
            var userId = GetUserId();
            var entries = await _journalService.GetAllEntriesAsync(userId, page, 10);
            var totalCount = await _journalService.GetAllEntriesAsync(userId, 1, int.MaxValue);

            var listModel = _mapper.Map<List<JournalEntryListViewModel>>(entries);
            var model = new JournalEntryListPageViewModel
            {
                Entries = listModel,
                CurrentPage = page,
                PageSize = 10,
                TotalCount = totalCount.Count()
            };

            return View(model);
        }

        public IActionResult Create() => View(new JournalEntryCreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JournalEntryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entry = _mapper.Map<Models.Models.JournalEntry>(model);
            entry.UserId = GetUserId();
            await _journalService.CreateEntryAsync(entry);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var entry = await _journalService.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();

            var model = _mapper.Map<JournalEntryEditViewModel>(entry);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JournalEntryEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entry = await _journalService.GetEntryByIdAsync(model.Id);
            if (entry == null) return NotFound();

            _mapper.Map(model, entry);
            await _journalService.UpdateEntryAsync(entry);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsPartial(Guid id)
        {
            var entry = await _journalService.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();

            var model = _mapper.Map<JournalEntryDetailViewModel>(entry);
            return PartialView("_JournalDetailsPartial", model);
        }

        public async Task<IActionResult> DeletePartial(Guid id)
        {
            var entry = await _journalService.GetEntryByIdAsync(id);
            if (entry == null) return NotFound();

            var model = _mapper.Map<JournalEntryDetailViewModel>(entry);
            return PartialView("_JournalDeletePartial", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _journalService.DeleteEntryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
