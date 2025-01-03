namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // Foreign key for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null;
    }
}