using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Database
{
    public class ExpenseTrackerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the relationship between User and Expense (One to Many)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade); // When a User is deleted, their Expenses are also deleted

            // Configuring the relationship between Category and Expense (One to Many)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // If a Category is deleted, the CategoryId in Expense is set to null

            // Ensuring no nulls for Category's Description property
            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .IsRequired(false); // Set to false if Description is optional

            // Indexes and other configurations if needed
            modelBuilder.Entity<Expense>()
                .HasIndex(e => e.Description)
                .HasDatabaseName("Index_Description");

            base.OnModelCreating(modelBuilder);
        }
    }
}