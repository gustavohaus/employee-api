using AutoMapper;
using Bogus;
using Employee.Application.Employee.ListEmployees;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using EmployeeEntity = Employee.Domain.Entities.Employee;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Employee.Tests.Application.Handlers
{
    public class ListEmployeesHandlerTests
    {
        private readonly IEmployeeRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly ListEmployeesHandler _handler;
        private readonly Faker _faker;

        public ListEmployeesHandlerTests()
        {
            _repositoryMock = Substitute.For<IEmployeeRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new ListEmployeesHandler(_repositoryMock, _mapperMock);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedEmployees_WhenEmployeesExist()
        {
            // Arrange
            var employees = GenerateEmployees(5);
            var pagedResult = (employees, employees.Count);
            var employeeResults = GenerateEmployeeResults(employees);

            _repositoryMock.GetPagedSalesAsync(
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<Guid?>(),
                Arg.Any<EmployeeRole>(),
                Arg.Any<CancellationToken>()
            ).Returns(pagedResult);

            _mapperMock.Map<List<EmployeeResult>>(employees).Returns(employeeResults);

            var command = new ListEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Role = EmployeeRole.Leader,
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                ManagerId = (Guid?)null
            };


            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.TotalCount.Should().Be(employees.Count);
            response.Employees.Should().HaveCount(employees.Count);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoEmployeesExist()
        {
            // Arrange
            var employees = new List<EmployeeEntity>();
            var pagedResult = (employees, 0);
            var employeeResults = new List<EmployeeResult>();

            _repositoryMock.GetPagedSalesAsync(
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime?>(),
                Arg.Any<DateTime?>(),
                Arg.Any<Guid?>(),
                Arg.Any<EmployeeRole>(),
                Arg.Any<CancellationToken>()
            ).Returns(pagedResult);

            _mapperMock.Map<List<EmployeeResult>>(employees).Returns(employeeResults);

            var command = new ListEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Role = EmployeeRole.Leader,
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                ManagerId = (Guid?)null               
            };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.TotalCount.Should().Be(0);
            response.Employees.Should().BeEmpty();
        }

        private List<EmployeeEntity> GenerateEmployees(int count)
        {
            return new Faker<EmployeeEntity>()
                .CustomInstantiator(f => new EmployeeEntity(
                    firstName: f.Name.FirstName(),
                    lastName: f.Name.LastName(),
                    email: f.Internet.Email(),
                    documentNumber: f.Random.Replace("###########"),
                    password: f.Internet.Password(),
                    birthDate: f.Date.Past(30, DateTime.Now.AddYears(-18)),
                    role: EmployeeRole.Employee
                ))
                .Generate(count);
        }

        private List<EmployeeResult> GenerateEmployeeResults(List<EmployeeEntity> employees)
        {
            var results = new List<EmployeeResult>();

            foreach (var employee in employees)
            {
                results.Add(new EmployeeResult
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DocumentNumber = employee.DocumentNumber,
                    Role = employee.Role,
                    Status = employee.Status,
                    BirthDate = employee.BirthDate,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt,
                    Manager =  new ManagerResult
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "",
                        LastName = "",
                        Email = "",
                        Role = EmployeeRole.Leader
                    }
                });
            }

            return results;
        }
    }
}