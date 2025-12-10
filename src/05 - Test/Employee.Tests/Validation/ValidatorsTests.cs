using Bogus;
using Employee.Application.Auth.AuthenticateUser;
using Employee.Application.Employee.CreateEmployee;
using Employee.Application.Employee.DeleteEmployee;
using Employee.Application.Employee.UpdateEmployee;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Employee.Tests.Validation
{
    public class ValidatorsTests
    {
        private readonly Faker _faker;

        public ValidatorsTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void AuthenticateUserCommandValidator_ShouldPass_WhenValidCommand()
        {
            // Arrange
            var validator = new AuthenticateUserCommandValidator();
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password()
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void AuthenticateUserCommandValidator_ShouldFail_WhenEmailIsEmpty()
        {
            // Arrange
            var validator = new AuthenticateUserCommandValidator();
            var command = new AuthenticateUserCommand
            {
                Email = "",
                Password = _faker.Internet.Password()
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public void AuthenticateUserCommandValidator_ShouldFail_WhenPasswordIsEmpty()
        {
            // Arrange
            var validator = new AuthenticateUserCommandValidator();
            var command = new AuthenticateUserCommand
            {
                Email = _faker.Internet.Email(),
                Password = ""
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Password);
        }

        [Fact]
        public void CreateEmployeeValidator_ShouldPass_WhenValidCommand()
        {
            // Arrange
            var validator = new CreateEmployeeValidator();
            var command = new CreateEmployeeCommand
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Admin,
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void CreateEmployeeValidator_ShouldFail_WhenFirstNameIsEmpty()
        {
            // Arrange
            var validator = new CreateEmployeeValidator();
            var command = new CreateEmployeeCommand
            {
                FirstName = "",
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                Password = _faker.Internet.Password(),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18))
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FirstName);
        }

        [Fact]
        public void DeleteEmployeeCommandValidator_ShouldPass_WhenValidCommand()
        {
            // Arrange
            var validator = new DeleteEmployeeCommandValidator();
            var command = new DeleteEmployeeCommand
            {
                EmployeeId = Guid.NewGuid()
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void DeleteEmployeeCommandValidator_ShouldFail_WhenEmployeeIdIsEmpty()
        {
            // Arrange
            var validator = new DeleteEmployeeCommandValidator();
            var command = new DeleteEmployeeCommand
            {
                EmployeeId = Guid.Empty
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.EmployeeId);
        }

        [Fact]
        public void UpdateEmployeeCommandValidator_ShouldPass_WhenValidCommand()
        {
            // Arrange
            var validator = new UpdateEmployeeCommandValidator();
            var command = new UpdateEmployeeCommand
            {
                Id = Guid.NewGuid(),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                Role = EmployeeRole.Employee
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void UpdateEmployeeCommandValidator_ShouldFail_WhenEmployeeIdIsEmpty()
        {
            // Arrange
            var validator = new UpdateEmployeeCommandValidator();
            var command = new UpdateEmployeeCommand
            {
                Id = Guid.Empty,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                DocumentNumber = _faker.Random.Replace("###########"),
                BirthDate = _faker.Date.Past(5, DateTime.Now.AddYears(-10)),
                //Role = EmployeeRole.Employee
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.BirthDate);
            result.ShouldHaveValidationErrorFor(c => c.Role);
        }
    }
}