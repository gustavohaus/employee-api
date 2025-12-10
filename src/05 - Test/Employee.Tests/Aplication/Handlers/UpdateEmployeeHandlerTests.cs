using AutoMapper;
using Bogus;
using Employee.Application.Employee.CreateEmployee;
using Employee.Application.Employee.UpdateEmployee;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EmployeeEntity = Employee.Domain.Entities.Employee;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Employee.Tests.Application.Handlers
{
    public class UpdateEmployeeHandlerTests
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly ILogger<UpdateEmployeeHandler> _loggerMock;
        private readonly IValidator<UpdateEmployeeCommand> _validatorMock;
        private readonly IMapper _mapperMock;
        private readonly UpdateEmployeeHandler _handler;
        private readonly Faker _faker;

        public UpdateEmployeeHandlerTests()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _loggerMock = Substitute.For<ILogger<UpdateEmployeeHandler>>();
            _validatorMock = Substitute.For<IValidator<UpdateEmployeeCommand>>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new UpdateEmployeeHandler(_repositoryMock, _loggerMock, _validatorMock, _mapperMock);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenValidCommand()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = GenerateEmployeeWithPhone(EmployeeRole.Leader);
            employee.Id = employeeId;
            var command = GenerateValidCommand(employeeId);

            _repositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(employee);
            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());


            _repositoryMock.UpdateAsync(employee, Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<UpdateEmployeeResult>(employee).Returns(new UpdateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
        }
        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenEmployedNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = GenerateEmployeeWithPhone(EmployeeRole.Leader);
            var command = GenerateValidCommand(employeeId);

            _repositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();
            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            await _repositoryMock.Received(0).UpdateAsync(employee, Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenValidCommandAddPhones()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = GenerateEmployeeWithoutPhone(EmployeeRole.Leader);
            var command = GenerateValidCommand(employeeId);
            employee.Id = employeeId;

            _repositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(employee);
            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _repositoryMock.UpdateAsync(employee, Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<UpdateEmployeeResult>(employee).Returns(new UpdateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            await _repositoryMock.Received(1).UpdateAsync(employee, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenValidCommandUpdatePhones()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = GenerateEmployeeWithPhone(EmployeeRole.Leader);
            var command = GenerateValidCommandToUpdatePhoneAndAdd(employeeId, employee.Phones.FirstOrDefault().Id);
            employee.Id = employeeId;

            _repositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(employee);
            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _repositoryMock.UpdateAsync(employee, Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<UpdateEmployeeResult>(employee).Returns(new UpdateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            await _repositoryMock.Received(1).UpdateAsync(employee, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenValidCommandRemovePhones()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = GenerateEmployeeWithPhone(EmployeeRole.Leader);
            var command = GenerateValidCommandToRemovePhone(employeeId, employee.Phones.FirstOrDefault().Id);
            employee.Id = employeeId;            

            _repositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(employee);
            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _repositoryMock.UpdateAsync(employee, Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<UpdateEmployeeResult>(employee).Returns(new UpdateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            await _repositoryMock.Received(1).UpdateAsync(employee, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new UpdateEmployeeCommand { Id = Guid.NewGuid() };
            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Id", "Invalid ID") });

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(validationResult);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        private UpdateEmployeeCommand GenerateValidCommand(Guid guid)
        {
            return new UpdateEmployeeCommand
            {
                Id = guid,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                Phones = new List<UpdateEmployeePhoneCommand>
        {
                new UpdateEmployeePhoneCommand
                {
                    Number = _faker.Phone.PhoneNumber(),
                    Type = PhoneType.Mobile,
                    IsPrimary = true
                },
                new UpdateEmployeePhoneCommand
                {
                    Number = _faker.Phone.PhoneNumber(),
                    Type = PhoneType.Home,
                    IsPrimary = false
                }
        }
            };
        }
        private UpdateEmployeeCommand GenerateValidCommandToRemovePhone(Guid guid, Guid PhoneId)
        {
            return new UpdateEmployeeCommand
            {
                Id = guid,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                Phones = new List<UpdateEmployeePhoneCommand>
        {
                new UpdateEmployeePhoneCommand
                {
                    Id = PhoneId,
                    Number = _faker.Phone.PhoneNumber(),
                    Type = PhoneType.Mobile,
                    IsPrimary = true
                },
        }
            };
        }
        private UpdateEmployeeCommand GenerateValidCommandToUpdatePhoneAndAdd(Guid guid, Guid PhoneId)
        {
            return new UpdateEmployeeCommand
            {
                Id = guid,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                Phones = new List<UpdateEmployeePhoneCommand>
        {
                new UpdateEmployeePhoneCommand
                {
                    Id = PhoneId,
                    Number = _faker.Phone.PhoneNumber(),
                    Type = PhoneType.Mobile,
                    IsPrimary = true
                },
                new UpdateEmployeePhoneCommand
                {                   
                    Number = _faker.Phone.PhoneNumber(),
                    Type = PhoneType.Mobile,
                    IsPrimary = false
                }
        }
            };
        }

        private EmployeeEntity GenerateEmployeeWithPhone(EmployeeRole role)
        {
            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                role: role
            );

            employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Mobile, true));
            employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Home, false));

            return employee;
        }
        private EmployeeEntity GenerateEmployeeWithoutPhone(EmployeeRole role)
        {
            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                role: role
            );

            //employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Mobile, true));
            //employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Home, false));
            //
            return employee;
        }
    }
}