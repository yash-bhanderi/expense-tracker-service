using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Controllers;
using ExpenseTracker.Database;
using ExpenseTracker.Dtos.Auth;
using ExpenseTracker.Models;
using System.Threading.Tasks;

public class AuthControllerTests
{
    private readonly Mock<ExpenseTrackerDbContext> _mockContext;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockContext = new Mock<ExpenseTrackerDbContext>();
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new AuthController(_mockContext.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task Register_MissingFields_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto { Email = "", Password = "", Name = "" };

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid input.", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_MissingFields_ReturnsBadRequest()
    {
        // Arrange
        var dto = new LoginDto { Email = "", Password = "" };

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid input.", badRequestResult.Value);
    }

    [Fact]
    public async Task GoogleLogin_MissingFields_ReturnsBadRequest()
    {
        // Arrange
        var dto = new GoogleLoginDto { GoogleId = "", Email = "", Name = "" };

        // Act
        var result = await _controller.GoogleLogin(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid input.", badRequestResult.Value);
    }

    [Fact]
    public async Task Register_WeakPassword_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@example.com", Password = "123", Name = "Test User" };
        _mockContext.Setup(c => c.Users.AnyAsync(u => u.Email == dto.Email)).ReturnsAsync(false);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Password is too weak.", badRequestResult.Value);
    }
}