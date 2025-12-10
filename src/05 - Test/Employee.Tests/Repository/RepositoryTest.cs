using Bogus;
using Employee.Data;
using Employee.Data.Repositories;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ValidationException = FluentValidation.ValidationException;
using NSubstitute.ReturnsExtensions;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Tests.Repository
{
    public class RepositoryTest
    {
        private readonly DefaultContext _dbContext;
        private readonly IEmployeeRepository _repository;
        private readonly Faker _faker;

        public RepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new DefaultContext(options);
            _repository = new EmployeeRepository(_dbContext);
            _faker = new Faker();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(employeeId, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = GenerateEmployee();
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(employee.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(employee, options => options.Excluding(e => e.Manager));
        }

        private EmployeeEntity GenerateEmployee()
        {
            return new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee
            );
        }
    }
}