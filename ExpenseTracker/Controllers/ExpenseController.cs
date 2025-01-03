using ExpenseTracker.Dtos.Expense;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ExpenseTracker.Database;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseController(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        // Get all expenses for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category) // Include category details
                .ToListAsync();

            return Ok(expenses);
        }

        // Get a specific expense by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expense = await _context.Expenses
                .Include(e => e.Category) // Include category details
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

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

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
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

            await _context.SaveChangesAsync();

            return Ok(expense);
        }

        // Delete an expense
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok("Expense deleted successfully.");
        }
    }
}
