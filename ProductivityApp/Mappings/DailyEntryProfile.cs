using AutoMapper;
using ProductivityApp.Models.Models;
using ProductivityApp.Web.ViewModels.DailyEntries;
using ProductivityApp.Web.ViewModels.DailyEntry;
using ProductivityApp.Web.ViewModels.Habits;
using ProductivityApp.Web.ViewModels.Journal;
using ProductivityApp.Web.ViewModels.Tasks;
using System.Linq;

namespace ProductivityApp.Web.Mappings
{
    public class DailyEntryProfile : Profile
    {
        public DailyEntryProfile()
        {
            CreateMap<DailyEntry, DailyEntryDetailViewModel>()
                .ForMember(dest => dest.Habits, opt => opt.MapFrom(src =>
                    src.HabitCompletions.Select(hc => new HabitListViewModel
                    {
                        Id = hc.Habit.Id,
                        Name = hc.Habit.Name,
                        Description = hc.Habit.Description,
                        Frequency = hc.Habit.Frequency,
                        TotalCompletions = hc.Habit.Completions.Count,
                        IsCompletedToday = hc.IsCompleted,
                        IsDeleted = hc.Habit.IsDeleted
                    }).ToList()
                ))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src =>
                    src.Tasks.Select(t => new TaskListViewModel
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        IsDeleted = t.IsDeleted
                    }).ToList()
                ))
                .ForMember(dest => dest.JournalEntries, opt => opt.MapFrom(src =>
                    src.JournalEntries.Select(j => new JournalEntryListViewModel
                    {
                        Id = j.Id,
                        Mood = j.Mood,
                        Note = j.Note,
                        Date = j.Date
                    }).ToList()
                ));

            CreateMap<DailyEntry, DailyEntryListViewModel>()
                .ForMember(dest => dest.HabitsCount, opt => opt.MapFrom(src => src.HabitCompletions.Count))
                .ForMember(dest => dest.TasksCount, opt => opt.MapFrom(src => src.Tasks.Count))
                .ForMember(dest => dest.JournalCount, opt => opt.MapFrom(src => src.JournalEntries.Count));
        }
    }
}
