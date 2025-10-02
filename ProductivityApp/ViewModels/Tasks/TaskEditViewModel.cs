using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Web.ViewModels.Tasks
{
    public class TaskEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

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

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }

        [Display(Name = "Completion Note")]
        public string? CompletionNote { get; set; }
    }
}
