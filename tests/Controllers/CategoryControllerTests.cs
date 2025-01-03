using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Controllers;
using ExpenseTracker.Services;
using ExpenseTracker.Dtos.Category;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new CategoryController(_mockCategoryService.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Id = 1, Name = "Food" },
            new CategoryDto { Id = 2, Name = "Transport" }
        };
        _mockCategoryService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CategoryDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetCategoryById_ExistingId_ReturnsOkResult_WithCategory()
    {
        // Arrange
        var category = new CategoryDto { Id = 1, Name = "Food" };
        _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _controller.GetCategoryById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CategoryDto>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Food", returnValue.Name);
    }

    [Fact]
    public async Task GetCategoryById_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(1)).ReturnsAsync((CategoryDto)null);

        // Act
        var result = await _controller.GetCategoryById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddCategory_ValidCategory_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var categoryCreateDto = new CategoryCreateDto { Name = "Food" };
        var category = new CategoryDto { Id = 1, Name = "Food" };
        _mockCategoryService.Setup(service => service.AddCategoryAsync(categoryCreateDto)).ReturnsAsync(category);

        // Act
        var result = await _controller.AddCategory(categoryCreateDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<CategoryDto>(createdAtActionResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Food", returnValue.Name);
    }

    [Fact]
    public async Task UpdateCategory_ExistingId_ReturnsNoContentResult()
    {
        // Arrange
        var categoryUpdateDto = new CategoryUpdateDto { Name = "Food" };
        _mockCategoryService.Setup(service => service.UpdateCategoryAsync(1, categoryUpdateDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateCategory(1, categoryUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCategory_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var categoryUpdateDto = new CategoryUpdateDto { Name = "Food" };
        _mockCategoryService.Setup(service => service.UpdateCategoryAsync(1, categoryUpdateDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateCategory(1, categoryUpdateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_ExistingId_ReturnsNoContentResult()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.DeleteCategoryAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.DeleteCategoryAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}