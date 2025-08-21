using Microsoft.EntityFrameworkCore;
using ProductivityApp.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Models.Models
{
    public class JournalEntry
    {
        [Key]
        [Display(Name = "Идентификатор на записа")]
        [Comment("Уникален идентификатор на дневниковия запис")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата на записа")]
        [Comment("Датата, на която е направен дневниковият запис")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Настроение")]
        [Comment("Настроението на потребителя за тази дата")]
        public MoodType Mood { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Бележка")]
        [Comment("Допълнителна бележка или описание за този ден")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Потребител")]
        [Comment("ID на потребителя, който е създал записа")]
        public string UserId { get; set; }

        [Display(Name = "Потребител")]
        [Comment("Потребителят, който е създал този дневников запис")]
        public ApplicationUser User { get; set; }
    }
}
