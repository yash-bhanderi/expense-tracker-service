using ExpenseTracker.Dtos.Expense;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ExpenseTracker.Database;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _context;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(ExpenseTrackerDbContext context, IExpenseRepository expenseRepository)
        {
            _context = context;
            _expenseRepository = expenseRepository;
        }

        // Get all expenses for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expenses = await _expenseRepository.GetExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        // Get a specific expense by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);

            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        // Add a new expense
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expense = new Expense
            {
                Amount = dto.Amount,
                Date = dto.Date,
                Description = dto.Description,
                CategoryId = dto.CategoryId, // Link to category
                UserId = userId             // Link to the logged-in user
            };

            await _expenseRepository.AddExpenseAsync(expense);

            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id });
        }

        // Update an existing expense
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
                return NotFound();

            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.Description = dto.Description;
            expense.CategoryId = dto.CategoryId; // Update category

            await _expenseRepository.UpdateExpenseAsync(expense);

            return Ok("Expense updated successfully: " + id);
        }

        // Delete an expense
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);

            if (expense == null)
                return NotFound();
            
            await _expenseRepository.DeleteExpenseAsync(id);

            return Ok("Expense deleted successfully: " + id);
        }
        
        // Search expenses
        [HttpGet("{description}")]
        public async Task<IActionResult> SearchExpense(string description)
        {
            var expenses = await _expenseRepository.SearchExpenseAsync(description);

            if (expenses == null)
                return Ok();
            
            return Ok(expenses);
        }
    }
}
