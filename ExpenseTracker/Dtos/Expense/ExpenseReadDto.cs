namespace ExpenseTracker.Dtos.Expense
{
    public class ExpenseReadDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}