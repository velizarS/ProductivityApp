using AutoMapper;
using ProductivityApp.Models.Models;
using ProductivityApp.Web.ViewModels.Journal;

namespace ProductivityApp.Web.Mappings
{
    public class JournalProfile : Profile
    {
        public JournalProfile()
        {
            CreateMap<JournalEntry, JournalEntryListViewModel>();
            CreateMap<JournalEntry, JournalEntryDetailViewModel>();
            CreateMap<JournalEntry, JournalEntryEditViewModel>();
            CreateMap<JournalEntryEditViewModel, JournalEntry>();
            CreateMap<JournalEntryCreateViewModel, JournalEntry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.DailyEntryId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
        }
    }
}
