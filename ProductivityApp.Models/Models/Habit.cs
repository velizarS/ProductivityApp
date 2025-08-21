using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class Habit
    {
        [Key]
        [Display(Name = "Идентификатор на навика")]
        [Comment("Уникален идентификатор на навика")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Име на навика")]
        [Comment("Името на навика, което потребителят задава")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Честота")]
        [Comment("Честота на изпълнение на навика (ежедневно, седмично и т.н.)")]
        public FrequencyType Frequency { get; set; }

        [Required]
        [Display(Name = "Потребител")]
        [Comment("ID на потребителя, който притежава навика")]
        public string UserId { get; set; }

        [Display(Name = "Потребител")]
        [Comment("Потребителят, който притежава навика")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Изпълнения на навика")]
        [Comment("Колекция от изпълнения на навика")]
        public ICollection<HabitCompletion> Completions { get; set; } = new List<HabitCompletion>();
    }
}
