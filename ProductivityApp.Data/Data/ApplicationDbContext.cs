using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.Models.Models;

namespace ProductivityApp.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitCompletion> HabitCompletions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<TaskM> Tasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Habit>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<HabitCompletion>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<JournalEntry>().HasQueryFilter(j => !j.IsDeleted);
            modelBuilder.Entity<TaskM>().HasQueryFilter(t => !t.IsDeleted); 

            modelBuilder.Entity<Habit>()
                .HasMany(h => h.Completions)
                .WithOne(c => c.Habit)
                .HasForeignKey(c => c.HabitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.User)
                .WithMany(u => u.JournalEntries)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HabitCompletion>()
                .HasOne(c => c.Habit)
                .WithMany(h => h.Completions)
                .HasForeignKey(c => c.HabitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskM>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks) 
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskM>()
                .HasOne(t => t.JournalEntry)
                .WithMany(j => j.Tasks) 
                .HasForeignKey(t => t.JournalEntryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
