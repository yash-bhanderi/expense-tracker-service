namespace ExpenseTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property for related expenses
        public ICollection<Expense> Expenses { get; set; }
    }
}