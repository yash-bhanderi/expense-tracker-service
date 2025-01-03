using ExpenseTracker.Database;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId)
        {
            return await _context.Expenses.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.Where(e => e.Id == id).FirstOrDefaultAsync();
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Expense>> SearchExpenseAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                return null; // Exit if the name is less than 3 characters
            }

            var expensesQuery = _context.Expenses.AsQueryable();

            // First, prioritize records where the description starts with the name
            var startingWithExpenses = expensesQuery
                .Where(e => e.Description.StartsWith(name))
                .Take(5); // Limit to 5 results

            // Then, prioritize records that contain the name (but not those that start with it)
            var containingExpenses = expensesQuery
                .Where(e => e.Description.Contains(name) && !e.Description.StartsWith(name))
                .Take(5);

            // Combine both queries, keeping the priority
            var result = startingWithExpenses.Concat(containingExpenses)
                .Distinct() // Remove duplicates
                .Take(5)     // Ensure no more than 5 results are returned
                .ToList();

            return result;
        }
    }
}