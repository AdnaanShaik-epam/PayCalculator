using Microsoft.EntityFrameworkCore;
using PayCalculator.Models;

namespace PayCalculator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<BreakEntry> BreakEntries { get; set; }
        public DbSet<SalaryInfo> SalaryInfos { get; set; }
        public DbSet<User> Users { get; set; } // Optional, if you use User model

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships and constraints
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.TimeEntries)
                .WithOne(te => te.Employee)
                .HasForeignKey(te => te.EmployeeId);

            modelBuilder.Entity<TimeEntry>()
                .HasMany(te => te.BreakEntries)
                .WithOne(be => be.TimeEntry)
                .HasForeignKey(be => be.TimeEntryId);

            modelBuilder.Entity<SalaryInfo>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId);

            // Optional: User-Employee relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithMany()
                .HasForeignKey(u => u.EmployeeId)
                .IsRequired(false);
        }
    }
}