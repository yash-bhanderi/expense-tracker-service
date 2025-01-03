namespace ExpenseTracker.Dtos.Expense
{
    public class ExpenseCreateDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; } // Link to Category
    }
}