using AutoMapper;
using Employee.Application.Auth.AuthenticateUser;
using Employee.WebApi.Controllers.Auth;
using Employee.WebApi.Controllers.Auth.AuthenticateUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Employee.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly IMediator _mediatorMock;
        private readonly IMapper _mapperMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = Substitute.For<IMediator>();
            _mapperMock = Substitute.For<IMapper>();
            _controller = new AuthController(_mediatorMock, _mapperMock);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new AuthenticateUserRequest { Email = "test@example.com", Password = "password123" };
            var command = new AuthenticateUserCommand { Email = request.Email, Password = request.Password };
            var result = new AuthenticateUserResult { Token = "valid-token", Id = Guid.NewGuid() };

            _mapperMock.Map<AuthenticateUserCommand>(request).Returns(command);
            _mediatorMock.Send(command, Arg.Any<CancellationToken>()).Returns(result);
            _mapperMock.Map<AuthenticateUserResponse>(result).Returns(new AuthenticateUserResponse { Token = result.Token });

            // Act
            var response = await _controller.Authenticate(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new AuthenticateUserRequest { Email = "test@example.com", Password = "wrongpassword" };
            var command = new AuthenticateUserCommand { Email = request.Email, Password = request.Password };

            _mapperMock.Map<AuthenticateUserCommand>(request).Returns(command);
            _mediatorMock.Send(command, Arg.Any<CancellationToken>()).Returns((AuthenticateUserResult)null);

            // Act
            var response = await _controller.Authenticate(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(response);
        }
    }
}
