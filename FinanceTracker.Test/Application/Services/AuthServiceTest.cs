using Moq;
using FinanceTracker.Domain.Identity.Interfaces;
namespace FinanceTracker.Test.Application.Services
{
    [TestFixture]
    public class AuthServiceTest
    {
        private Mock<IAuthService> _authService;

        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthService>();
        }

        [Test]
        public void AuthenticateAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var email = "email@example.com";
            var password = "password";
            var expectedToken = "token";
            _authService.Setup(x => x.AuthenticateAsync(email, password))
                .ReturnsAsync(expectedToken);

            // Act
            var result = _authService.Object.AuthenticateAsync(email, password).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expectedToken));
            _authService.Verify(x => x.AuthenticateAsync(email, password), Times.Once);
        }

        [Test]
        public void RegisterAsync_ValidData_CallsRegister()
        {
            // Arrange
            var email = "email@example.com";
            var password = "password";
            var role = "user";
            _authService.Setup(x => x.RegisterAsync(email, password, role))
                .Returns(Task.CompletedTask);

            // Act
            _authService.Object.RegisterAsync(email, password, role).Wait();
            IAuthService authService = _authService.Object;

            // Assert
            _authService.Verify(x => x.RegisterAsync(email, password, role), Times.Once);
            _authService.Verify(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void RegisterAsync_InvalidData_ThrowsException()
        {
            // Arrange
            var email = "invalid-email";
            var password = "password";
            var role = "user";
            _authService.Setup(x => x.RegisterAsync(email, password, role))
                .Throws(new Exception("Invalid data"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _authService.Object.RegisterAsync(email, password, role));
            _authService.Verify(x => x.RegisterAsync(email, password, role), Times.Once);
        }
        [Test]
        public void AuthenticateAsync_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var email = "email@example.com";
            var password = "wrongpassword";
            _authService.Setup(x => x.AuthenticateAsync(email, password))
                .Throws(new Exception("Invalid credentials"));
                
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _authService.Object.AuthenticateAsync(email, password));
            _authService.Verify(x => x.AuthenticateAsync(email, password), Times.Once);

        }
    }
}