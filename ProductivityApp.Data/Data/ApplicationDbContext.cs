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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Habit entity
            modelBuilder.Entity<Habit>()
                .HasMany(h => h.Completions)
                .WithOne(c => c.Habit)
                .HasForeignKey(c => c.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure JournalEntry entity
            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.User)
                .WithMany(u => u.JournalEntries)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure HabitCompletion entity
            modelBuilder.Entity<HabitCompletion>()
                .HasOne(c => c.Habit)
                .WithMany(h => h.Completions)
                .HasForeignKey(c => c.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
