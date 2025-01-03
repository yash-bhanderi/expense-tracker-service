using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Controllers;
using ExpenseTracker.Repositories;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _controller = new CategoryController(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Food", Description = "Food related expenses" },
            new Category { Id = 2, Name = "Transport", Description = "Transport related expenses" }
        };
        _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<Category>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetCategory_ExistingId_ReturnsOkResult_WithCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Food", Description = "Food related expenses" };
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Category>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Food", returnValue.Name);
    }

    [Fact]
    public async Task GetCategory_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCategory_ValidCategory_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var categoryCreateDto = new CategoryCreateDto { Name = "Food", Description = "Food related expenses" };
        var category = new Category { Id = 1, Name = "Food", Description = "Food related expenses" };
        _mockCategoryRepository.Setup(repo => repo.CategoryExistsAsync(categoryCreateDto.Name)).ReturnsAsync(false);
        _mockCategoryRepository.Setup(repo => repo.AddCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateCategory(categoryCreateDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<Category>(createdAtActionResult.Value);
        Assert.Equal("Food", returnValue.Name);
    }

    [Fact]
    public async Task CreateCategory_DuplicateCategory_ReturnsBadRequest()
    {
        // Arrange
        var categoryCreateDto = new CategoryCreateDto { Name = "Food", Description = "Food related expenses" };
        _mockCategoryRepository.Setup(repo => repo.CategoryExistsAsync(categoryCreateDto.Name)).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateCategory(categoryCreateDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCategory_ExistingId_ReturnsOkResult_WithUpdatedCategory()
    {
        // Arrange
        var categoryUpdateDto = new CategoryCreateDto { Name = "Food", Description = "Updated description" };
        var category = new Category { Id = 1, Name = "Food", Description = "Food related expenses" };
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync(category);
        _mockCategoryRepository.Setup(repo => repo.UpdateCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateCategory(1, categoryUpdateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Category>(okResult.Value);
        Assert.Equal("Updated description", returnValue.Description);
    }

    [Fact]
    public async Task UpdateCategory_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var categoryUpdateDto = new CategoryCreateDto { Name = "Food", Description = "Updated description" };
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);

        // Act
        var result = await _controller.UpdateCategory(1, categoryUpdateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Food", Description = "Food related expenses" };
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync(category);
        _mockCategoryRepository.Setup(repo => repo.DeleteCategoryAsync(category)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Category deleted successfully.", okResult.Value);
    }

    [Fact]
    public async Task DeleteCategory_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}