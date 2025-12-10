using AutoMapper;
using Bogus;
using Employee.Application.Employee.GetEmployee;
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
    public class GetEmployeeHandlerTests
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly ILogger<GetEmployeeHandler> _loggerMock;
        private readonly GetEmployeeHandler _handler;
        private readonly Faker _faker;

        public GetEmployeeHandlerTests()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _loggerMock = Substitute.For<ILogger<GetEmployeeHandler>>();
            _handler = new GetEmployeeHandler(_repositoryMock, _mapperMock, _loggerMock);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var phoneId = Guid.NewGuid();

            var employee = new EmployeeEntity(
                firstName: _faker.Name.FirstName(),
                lastName: _faker.Name.LastName(),
                email: _faker.Internet.Email(),
                documentNumber: _faker.Random.Replace("###########"),
                password: _faker.Internet.Password(),
                birthDate: _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                role: EmployeeRole.Employee
            )
            {
                Id = employeeId,
                Manager = new EmployeeEntity(
                    firstName: _faker.Name.FirstName(),
                    lastName: _faker.Name.LastName(),
                    email: _faker.Internet.Email(),
                    documentNumber: _faker.Random.Replace("###########"),
                    password: _faker.Internet.Password(),
                    birthDate: _faker.Date.Past(40, DateTime.Now.AddYears(-18)),
                    role: EmployeeRole.Leader
                )
                {
                    Id = managerId
                }
            };

            employee.AddPhones(new Phone(null, _faker.Phone.PhoneNumber(), PhoneType.Mobile, true) { Id = phoneId });

            var result = new GetEmployeeResult
            {
                Id = employeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DocumentNumber = employee.DocumentNumber,
                Role = employee.Role,
                Status = employee.Status,
                BirthDate = employee.BirthDate,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
                Manager = new GetEmployeeManagerResult
                {
                    Id = managerId,
                    FirstName = employee.Manager.FirstName,
                    LastName = employee.Manager.LastName,
                    Email = employee.Manager.Email,
                    Role = employee.Manager.Role
                },
                Phones = new List<GetEmployeePhoneResult>
        {
            new GetEmployeePhoneResult
            {
                Id = phoneId,
                Number = employee.Phones.First().Number,
                Type = employee.Phones.First().Type,
                IsPrimary = employee.Phones.First().IsPrimary
            }
        }
            };

            // Configuração do mock do repositório
            _repositoryMock.GetByIdAsync(employeeId, Arg.Any<CancellationToken>()).Returns(employee);

            // Configuração do mock do mapper
            _mapperMock.Map<GetEmployeeResult>(employee).Returns(result);

            var command = new GetEmployeeCommand { EmployeeId = employeeId };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(result.Id);
            response.FirstName.Should().Be(result.FirstName);
            response.LastName.Should().Be(result.LastName);
            response.Manager.Should().NotBeNull();
            response.Manager.Id.Should().Be(result.Manager.Id);
            response.Phones.Should().HaveCount(1);
            response.Phones.First().Id.Should().Be(result.Phones.First().Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _repositoryMock.GetByIdAsync(employeeId, Arg.Any<CancellationToken>()).Returns((EmployeeEntity)null);

            var command = new GetEmployeeCommand { EmployeeId = employeeId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}