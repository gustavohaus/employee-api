using AutoMapper;
using Azure;
using Bogus;
using Employee.Application.Auth.AuthenticateUser;
using Employee.Application.Employee.CreateEmployee;
using Employee.Common.Validation;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using EmployeeEntity = Employee.Domain.Entities.Employee;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Employee.Tests.Aplication.Handlers
{
    public class AuthenticateUserHandlerTest
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly IJwtTokenGenerator _jwtTokenGeneratorMock;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly AuthenticateUserHandler _handler;
        private readonly Faker _faker;

        public AuthenticateUserHandlerTest()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _jwtTokenGeneratorMock = Substitute.For<IJwtTokenGenerator>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _handler = new AuthenticateUserHandler(_repositoryMock, _jwtTokenGeneratorMock, _passwordHasherMock);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldReturnAuthenticateUserResult_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                command.Email,
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee
            );

            _repositoryMock.GetEmployeeByEmail(command.Email, Arg.Any<CancellationToken>()).Returns(employee);
            _passwordHasherMock.VerifyPassword(command.Password, employee.Password).Returns(true);

            var token = _faker.Random.AlphaNumeric(50);
            _jwtTokenGeneratorMock.GenerateToken(employee.Id, employee.Email, employee.Role.ToString()).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be(token);
            result.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            _repositoryMock.GetEmployeeByEmail(command.Email, Arg.Any<CancellationToken>()).ReturnsNull();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                command.Email,
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee
            );

            _repositoryMock.GetEmployeeByEmail(command.Email, Arg.Any<CancellationToken>()).Returns(employee);
            _passwordHasherMock.VerifyPassword(command.Password, employee.Password).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldGenerateToken_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                command.Email,
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee
            );

            _repositoryMock.GetEmployeeByEmail(command.Email, Arg.Any<CancellationToken>()).Returns(employee);
            _passwordHasherMock.VerifyPassword(command.Password, employee.Password).Returns(true);

            var token = _faker.Random.AlphaNumeric(50);
            _jwtTokenGeneratorMock.GenerateToken(employee.Id, employee.Email, employee.Role.ToString()).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _jwtTokenGeneratorMock.Received(1).GenerateToken(employee.Id, employee.Email, employee.Role.ToString());
            result.Token.Should().Be(token);
        }
    }
}