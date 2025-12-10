using AutoMapper;
using Azure;
using Bogus;
using Employee.Application.Employee.CreateEmployee;
using Employee.Common.Validation;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using EmployeeEntity = Employee.Domain.Entities.Employee;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Employee.Tests.Application.Handlers
{
    public class CreateEmployeeHandlerTests
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly ILogger<CreateEmployeeHandler> _loggerMock;
        private readonly IValidator<CreateEmployeeCommand> _validatorMock;
        private readonly IMapper _mapperMock;
        private readonly CreateEmployeeHandler _handler;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly Faker _faker;

        public CreateEmployeeHandlerTests()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _loggerMock = Substitute.For<ILogger<CreateEmployeeHandler>>();
            _validatorMock = Substitute.For<IValidator<CreateEmployeeCommand>>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new CreateEmployeeHandler(_repositoryMock, _mapperMock, _loggerMock, _validatorMock, _passwordHasherMock);
            _faker = new Faker();
        }


        [Fact]
        public async Task Handle_ShouldCreateEmployeeByAdmin_WhenValidCommandWithoutManagerId()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                AuthenticateUser = Guid.NewGuid()
            };

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var authenticatedUser = GenerateEmployee(EmployeeRole.Admin);
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);

            var hashedPassword = _faker.Internet.Password();
            _passwordHasherMock.HashPassword(command.Password).Returns(hashedPassword);

            var employee = new EmployeeEntity(
                command.FirstName,
                command.LastName,
                command.Email,
                command.DocumentNumber,
                hashedPassword,
                command.BirthDate,
                command.Role
            );

            _repositoryMock.CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<CreateEmployeeResult>(employee).Returns(new CreateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployeeByAdmin_WhenValidCommandWithManagerId()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                AuthenticateUser = Guid.NewGuid(),
                ManagerId = Guid.NewGuid(),
            };

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var authenticatedUser = GenerateEmployee(EmployeeRole.Admin);
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);

            var managerUser = GenerateEmployee(EmployeeRole.Leader);
            _repositoryMock.GetByIdAsync(command.ManagerId.Value, Arg.Any<CancellationToken>()).Returns(managerUser);

            var hashedPassword = _faker.Internet.Password();
            _passwordHasherMock.HashPassword(command.Password).Returns(hashedPassword);

            var employee = new EmployeeEntity(
                command.FirstName,
                command.LastName,
                command.Email,
                command.DocumentNumber,
                hashedPassword,
                command.BirthDate,
                command.Role,
                managerUser
            );

            _repositoryMock.CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<CreateEmployeeResult>(employee).Returns(new CreateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = GenerateInvalidCommand();

            var validationResult = new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("FirstName", "First name is required") });

            // Escrever mais validations;

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(validationResult);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            await _repositoryMock.DidNotReceive().CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidCommandAndManagerIsNullAndUserIsNotAdmin()
        {
            // Arrange
            var command = GenerateValidCommand();
            command.ManagerId = null;

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());

            var authenticatedUser = GenerateEmployee(EmployeeRole.Leader);
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            await _repositoryMock.DidNotReceive().CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenValidCommandAndManagerIsNullAndUserIsAdmin()
        {
            // Arrange
            var command = GenerateValidCommand();
            command.ManagerId = null;

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                 .Returns(new FluentValidation.Results.ValidationResult());

            var authenticatedUser = GenerateEmployee(EmployeeRole.Admin);
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);

            var hashedPassword = _faker.Internet.Password();
            _passwordHasherMock.HashPassword(command.Password).Returns(hashedPassword);

            var employee = GenerateEmployee(command.Role);

            _repositoryMock.CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<CreateEmployeeResult>(employee).Returns(new CreateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenManagerIsProvidedAndUserIsAdmin()
        {
            // Arrange
            var command = GenerateValidCommand();

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var authenticatedUser = GenerateEmployee(EmployeeRole.Admin);
            var manager = GenerateEmployee(EmployeeRole.Leader);

            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);
            _repositoryMock.GetByIdAsync(command.ManagerId.Value, Arg.Any<CancellationToken>()).Returns(manager);

            var hashedPassword = _faker.Internet.Password();
            _passwordHasherMock.HashPassword(command.Password).Returns(hashedPassword);

            var employee = GenerateEmployee(command.Role);

            _repositoryMock.CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<CreateEmployeeResult>(employee).Returns(new CreateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenManagerIsProvidedAndEmployeeHasLowerRole()
        {
            // Arrange
            var command = GenerateValidCommand();

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
             .Returns(new FluentValidation.Results.ValidationResult());


            var authenticatedUser = GenerateEmployee(EmployeeRole.Leader);
            var manager = authenticatedUser;
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);
            _repositoryMock.GetByIdAsync(command.ManagerId.Value, Arg.Any<CancellationToken>()).Returns(manager);

            var hashedPassword = _faker.Internet.Password();
            _passwordHasherMock.HashPassword(command.Password).Returns(hashedPassword);

            var employee = GenerateEmployee(command.Role, manager);

            _repositoryMock.CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>()).Returns(employee);
            _mapperMock.Map<CreateEmployeeResult>(employee).Returns(new CreateEmployeeResult { Id = employee.Id });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(employee.Id);
        }

        [Fact] 
        public async Task Handle_ShouldThrowValidationException_WhenManagerIsProvidedAndEmployeeHasHigherRole()
        {
            // Arrange
            var command = GenerateValidCommand();

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());


            var authenticatedUser = GenerateEmployee(EmployeeRole.Employee);
            var employee = GenerateEmployee(EmployeeRole.Employee);
   
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);
            _repositoryMock.GetByIdAsync(command.ManagerId.Value, Arg.Any<CancellationToken>()).Returns(employee);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert            
            await act.Should().ThrowAsync<ValidationException>();
            await _repositoryMock.DidNotReceive().CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact] 
        public async Task Handle_ShouldThrowValidationException_WhenEmployeeAlreadyExists()
        {
            // Arrange
            var command = GenerateValidCommand();

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());


            var authenticatedUser = GenerateEmployee(EmployeeRole.Employee);
            var employee = GenerateEmployee(EmployeeRole.Employee);
   
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>()).Returns(authenticatedUser);
            _repositoryMock.GetByIdAsync(command.ManagerId.Value, Arg.Any<CancellationToken>()).Returns(employee);
            _repositoryMock.GetEmployeeByEmailOrCpf(command.Email, command.DocumentNumber, Arg.Any<CancellationToken>())
                .Returns(employee);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<ValidationException>()
                .Where(ex => ex.Message.Contains("User already"));
        }
        [Fact] 
        public async Task Handle_ShouldThrowValidationException_WhenUserAuthenticatedNotExists()
        {
            // Arrange
            var command = GenerateValidCommand();

            _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());


            var authenticatedUser = GenerateEmployee(EmployeeRole.Employee);
            var employee = GenerateEmployee(EmployeeRole.Employee);
   
            _repositoryMock.GetByIdAsync(command.AuthenticateUser, Arg.Any<CancellationToken>())
                .ReturnsNull();


            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<ValidationException>();
            await _repositoryMock.DidNotReceive().CreateAsync(Arg.Any<EmployeeEntity>(), Arg.Any<CancellationToken>());
        }

        private CreateEmployeeCommand GenerateValidCommand()
        {
            var guid = Guid.NewGuid();
            return new CreateEmployeeCommand
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee,
                AuthenticateUser = guid,
                ManagerId = guid,
                Phones = new List<CreateEmployeePhoneCommand>
        {
            new CreateEmployeePhoneCommand
            {
                Number = _faker.Phone.PhoneNumber(),
                Type = PhoneType.Mobile,
                IsPrimary = true
            },
            new CreateEmployeePhoneCommand
            {
                Number = _faker.Phone.PhoneNumber(),
                Type = PhoneType.Home,
                IsPrimary = false
            }
        }
            };
        }
        private CreateEmployeeCommand GenerateInvalidCommand()
        {
            var guid = Guid.NewGuid();
            return new CreateEmployeeCommand
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-10)),
                Role = EmployeeRole.Employee,
                AuthenticateUser = guid,
                ManagerId = guid,
                Phones = new List<CreateEmployeePhoneCommand>
        {
            new CreateEmployeePhoneCommand
            {
                Number = _faker.Phone.PhoneNumber(),
                Type = PhoneType.Mobile,
                IsPrimary = true
            },
            new CreateEmployeePhoneCommand
            {
                Number = _faker.Phone.PhoneNumber(),
                Type = PhoneType.Home,
                IsPrimary = false
            }
        }
            };
        }

        private EmployeeEntity GenerateEmployee(EmployeeRole role, EmployeeEntity? manager = null)
        {
            var employee = new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                role: role,
                manager
            );

            employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Mobile, true));
            employee.AddPhones(new Phone(employee, _faker.Phone.PhoneNumber(), PhoneType.Home, false));

            return employee;
        }
    }
}