using Bogus;
using Employee.Application.Employee.DeleteEmployee;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ValidationException = FluentValidation.ValidationException;
using NSubstitute.ReturnsExtensions;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Tests.Application.Handlers
{
    public class DeleteEmployeeHandlerTests
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly ILogger<DeleteEmployeeHandler> _loggerMock;
        private readonly IValidator<DeleteEmployeeCommand> _validatorMock;
        private readonly DeleteEmployeeHandler _handler;
        private readonly Faker _faker;

        public DeleteEmployeeHandlerTests()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _loggerMock = Substitute.For<ILogger<DeleteEmployeeHandler>>();
            _handler = new DeleteEmployeeHandler(_repositoryMock, _loggerMock);
            _faker = new Faker();
            _validatorMock = Substitute.For<IValidator<DeleteEmployeeCommand>>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                 _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee);

            employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Mobile, true));

            _repositoryMock.GetByIdAsync(employeeId, Arg.Any<CancellationToken>()).Returns(employee);

            var command = new DeleteEmployeeCommand { EmployeeId = employeeId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _repositoryMock.Received(1).DeleteAsync(employee, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _repositoryMock.GetByIdAsync(employeeId, Arg.Any<CancellationToken>()).ReturnsNull();

            var command = new DeleteEmployeeCommand { EmployeeId = employeeId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"Employee - {employeeId} not found.");
            await _repositoryMock.DidNotReceive().DeleteAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { };

            var validationResult = new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("EmployeeId", "Employee ID is required") });

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(validationResult);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            await _repositoryMock
             .DidNotReceive()
             .DeleteAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }
    }
}