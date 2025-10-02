using AutoMapper;
using ProductivityApp.Models.Models;
using ProductivityApp.Web.ViewModels.Tasks;

namespace ProductivityApp.Web.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskM, TaskListViewModel>();
            CreateMap<TaskM, TaskDetailViewModel>();
            CreateMap<TaskM, TaskEditViewModel>();
            CreateMap<TaskCreateViewModel, TaskM>();
            CreateMap<TaskEditViewModel, TaskM>();
        }
    }
}
