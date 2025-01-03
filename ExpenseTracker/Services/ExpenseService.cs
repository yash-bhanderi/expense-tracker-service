using ExpenseTracker.Dtos.Expense;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // Get all expenses for a user
        public async Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
        {
            return await _expenseRepository.GetExpensesByUserIdAsync(userId);
        }

        // Get a specific expense by ID
        public async Task<Expense> GetExpenseByIdAsync(int id, int userId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null || expense.UserId != userId)
                throw new Exception("Expense not found or access denied.");

            return expense;
        }

        // Add a new expense
        public async Task<Expense> AddExpenseAsync(ExpenseCreateDto dto, int userId)
        {
            var expense = new Expense
            {
                Amount = dto.Amount,
                Date = dto.Date,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            await _expenseRepository.AddExpenseAsync(expense);
            return expense;
        }

        // Update an existing expense
        public async Task<Expense> UpdateExpenseAsync(int id, ExpenseUpdateDto dto, int userId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null || expense.UserId != userId)
                throw new Exception("Expense not found or access denied.");

            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Description = dto.Description;
            expense.CategoryId = dto.CategoryId;

            await _expenseRepository.UpdateExpenseAsync(expense);
            return expense;
        }

        // Delete an expense
        public async Task DeleteExpenseAsync(int id, int userId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            if (expense == null || expense.UserId != userId)
                throw new Exception("Expense not found or access denied.");

            await _expenseRepository.DeleteExpenseAsync(expense);
        }
    }
}
