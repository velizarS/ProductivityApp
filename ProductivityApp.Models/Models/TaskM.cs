using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductivityApp.Models.Models
{
    public class TaskM
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(100, ErrorMessage = "Заглавието не може да е по-дълго от 100 символа.")]
        [Display(Name = "Заглавие на задачата")]
        [Comment("Основното заглавие на задачата")]
        public string Title { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Описанието не може да е по-дълго от 500 символа.")]
        [Display(Name = "Описание")]
        [Comment("Допълнително описание на задачата, ако е необходимо")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Крайната дата е задължителна.")]
        [Display(Name = "Краен срок")]
        [Comment("Дата и час, до която задачата трябва да бъде изпълнена")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Изпълнена")]
        [Comment("Статус на задачата: изпълнена или не")]
        public bool IsCompleted { get; set; }

        [Required]
        [Display(Name = "Потребител")]
        [Comment("Идентификатор на потребителя, на когото принадлежи задачата")]
        public string UserId { get; set; } = null!;

        [Display(Name = "Потребител")]
        [Comment("Навигационно свойство към потребителя")]
        public ApplicationUser User { get; set; } = null!;

        [Display(Name = "Свързан дневник")]
        [Comment("Опционална връзка към дневников запис")]
        public Guid? JournalEntryId { get; set; }

        [Display(Name = "Дневников запис")]
        [Comment("Навигационно свойство към JournalEntry")]
        public JournalEntry? JournalEntry { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
