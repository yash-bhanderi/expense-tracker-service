namespace ExpenseTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string GoogleId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property for related expenses
        public ICollection<Expense> Expenses { get; set; }
    }
}