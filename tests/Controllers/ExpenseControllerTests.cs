using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Controllers;
using ExpenseTracker.Services;
using ExpenseTracker.Dtos.Expense;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ExpenseControllerTests
{
    private readonly Mock<IExpenseService> _mockExpenseService;
    private readonly ExpenseController _controller;

    public ExpenseControllerTests()
    {
        _mockExpenseService = new Mock<IExpenseService>();
        _controller = new ExpenseController(_mockExpenseService.Object);
    }

    [Fact]
    public async Task GetExpenses_ReturnsOkResult_WithListOfExpenses()
    {
        // Arrange
        var expenses = new List<ExpenseDto>
        {
            new ExpenseDto { Id = 1, Amount = 100, Description = "Groceries" },
            new ExpenseDto { Id = 2, Amount = 50, Description = "Transport" }
        };
        _mockExpenseService.Setup(service => service.GetExpensesAsync()).ReturnsAsync(expenses);

        // Act
        var result = await _controller.GetExpenses();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ExpenseDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetExpenseById_ExistingId_ReturnsOkResult_WithExpense()
    {
        // Arrange
        var expense = new ExpenseDto { Id = 1, Amount = 100, Description = "Groceries" };
        _mockExpenseService.Setup(service => service.GetExpenseByIdAsync(1)).ReturnsAsync(expense);

        // Act
        var result = await _controller.GetExpenseById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ExpenseDto>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(100, returnValue.Amount);
        Assert.Equal("Groceries", returnValue.Description);
    }

    [Fact]
    public async Task GetExpenseById_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockExpenseService.Setup(service => service.GetExpenseByIdAsync(1)).ReturnsAsync((ExpenseDto)null);

        // Act
        var result = await _controller.GetExpenseById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddExpense_ValidExpense_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var expenseCreateDto = new ExpenseCreateDto { Amount = 100, Description = "Groceries" };
        var expense = new ExpenseDto { Id = 1, Amount = 100, Description = "Groceries" };
        _mockExpenseService.Setup(service => service.AddExpenseAsync(expenseCreateDto)).ReturnsAsync(expense);

        // Act
        var result = await _controller.AddExpense(expenseCreateDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<ExpenseDto>(createdAtActionResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(100, returnValue.Amount);
        Assert.Equal("Groceries", returnValue.Description);
    }

    [Fact]
    public async Task UpdateExpense_ExistingId_ReturnsNoContentResult()
    {
        // Arrange
        var expenseUpdateDto = new ExpenseUpdateDto { Amount = 150, Description = "Groceries" };
        _mockExpenseService.Setup(service => service.UpdateExpenseAsync(1, expenseUpdateDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateExpense(1, expenseUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateExpense_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var expenseUpdateDto = new ExpenseUpdateDto { Amount = 150, Description = "Groceries" };
        _mockExpenseService.Setup(service => service.UpdateExpenseAsync(1, expenseUpdateDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateExpense(1, expenseUpdateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteExpense_ExistingId_ReturnsNoContentResult()
    {
        // Arrange
        _mockExpenseService.Setup(service => service.DeleteExpenseAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteExpense(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteExpense_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockExpenseService.Setup(service => service.DeleteExpenseAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteExpense(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}