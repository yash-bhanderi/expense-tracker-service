using System.Security.Cryptography;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Controllers;
using ExpenseTracker.Repositories;
using ExpenseTracker.Dtos.Auth;
using ExpenseTracker.Models;
using System.Threading.Tasks;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new AuthController(_mockUserRepository.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task Register_UserAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@example.com", Password = "password", Name = "Test User" };
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(dto.Email)).ReturnsAsync(new User());

        // Act
        var result = await _controller.Register(dto);

        // Assert7
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User already exists.", badRequestResult.Value);
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsOk()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@example.com", Password = "password", Name = "Test User" };
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(dto.Email)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.Register(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully.", okResult.Value);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@example.com", Password = "wrongpassword" };
        _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(dto.Email)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
    }

    
}