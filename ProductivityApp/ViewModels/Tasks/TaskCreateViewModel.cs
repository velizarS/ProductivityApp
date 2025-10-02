using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Web.ViewModels.Tasks
{
    public class TaskCreateViewModel
    {
        [Required]
        [MaxLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
    }
}
