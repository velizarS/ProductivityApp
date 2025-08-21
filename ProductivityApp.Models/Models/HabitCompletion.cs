using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class HabitCompletion
    {
        [Key]
        [Display(Name = "Идентификатор на изпълнението")]
        [Comment("Уникален идентификатор на записа за изпълнение на навика")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Навик")]
        [Comment("ID на навика, за който се отбелязва изпълнението")]
        public int HabitId { get; set; }

        [Display(Name = "Навик")]
        [Comment("Навикът, за който се отбелязва изпълнението")]
        public Habit Habit { get; set; }

        [Required]
        [Display(Name = "Дата на изпълнението")]
        [Comment("Датата, на която е отбелязано изпълнението на навика")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Изпълнено")]
        [Comment("Отбелязва дали навикът е изпълнен на тази дата")]
        public bool IsCompleted { get; set; }
    }
}
