using AutoMapper;
using ProductivityApp.Models.Models;
using ProductivityApp.Web.ViewModels.Habits;
using System.Linq;

namespace ProductivityApp.Web.Mappings
{
    public class HabitProfile : Profile
    {
        public HabitProfile()
        {
            CreateMap<Habit, HabitListViewModel>()
                .ForMember(dest => dest.TotalCompletions, opt => opt.MapFrom(src => src.Completions.Count));

            CreateMap<Habit, HabitDetailViewModel>()
                .ForMember(dest => dest.TotalCompletions, opt => opt.MapFrom(src => src.Completions.Count))
                .ForMember(dest => dest.Completions, opt => opt.MapFrom(src => src.Completions.Select(c => c.Date).ToList()));

            CreateMap<Habit, HabitEditViewModel>();
            CreateMap<HabitCreateViewModel, Habit>();
            CreateMap<HabitEditViewModel, Habit>();
        }
    }
}
