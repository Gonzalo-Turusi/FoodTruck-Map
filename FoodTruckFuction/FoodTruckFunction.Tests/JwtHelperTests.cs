using FoodTruckFuction.Interfaces;
using FoodTruckFuction.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Security.Claims;

namespace FoodTruckFunction.Tests
{
    public class JwtHelperTests
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly Mock<IEnvironmentService> _mockEnvironmentService;

        public JwtHelperTests()
        {
            _mockEnvironmentService = new Mock<IEnvironmentService>();
            _mockEnvironmentService.Setup(e => e.GetVariable("SECRET_KEY")).Returns("MySuperMegaSecretKeyWithMoreThan32Characters");

            _jwtHelper = new JwtHelper(_mockEnvironmentService.Object);
        }

        [Fact]
        public void GenerateToken_ReturnsValidToken()
        {
            // Arrange
            var username = "testuser";
            var role = "admin";

            // Act
            var token = _jwtHelper.GenerateToken(username, role);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void ValidateToken_ValidToken_ReturnsClaimsPrincipal()
        {
            // Arrange
            var token = _jwtHelper.GenerateToken("testuser", "admin");

            // Act
            var claimsPrincipal = _jwtHelper.ValidateToken(token);

            // Assert
            Assert.NotNull(claimsPrincipal);
            Assert.Equal("testuser", claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal("admin", claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value);
        }

        [Fact]
        public void ValidateToken_InvalidToken_ThrowsException()
        {
            // Arrange
            var invalidToken = "header.payload.signature";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _jwtHelper.ValidateToken(invalidToken));
            Assert.Contains("IDX12729", exception.Message);
        }

        [Fact]
        public void ValidateToken_EmptySecretKey_ThrowsException()
        {
            // Arrange
            _mockEnvironmentService.Setup(e => e.GetVariable("SECRET_KEY")).Returns(string.Empty);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _jwtHelper.ValidateToken("some-token"));
            Assert.Equal("SECRET_KEY is not set.", exception.Message);
        }

        [Fact]
        public void GenerateToken_EmptySecretKey_ThrowsException()
        {
            // Arrange
            _mockEnvironmentService.Setup(e => e.GetVariable("SECRET_KEY")).Returns(string.Empty);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _jwtHelper.GenerateToken("testuser", "admin"));
            Assert.Equal("SECRET_KEY is not set.", exception.Message);
        }
    }
}
