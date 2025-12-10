using Bogus;
using Employee.Domain.Entities;
using FluentAssertions;
using EmployeeEntity = Employee.Domain.Entities.Employee;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Employee.Tests.Domain
{
    public class EmployeeTest
    {
        private readonly Faker _faker;

        public EmployeeTest()
        {
            _faker = new Faker();
        }

        [Fact]
        public void UpdatePhone_ShouldThrowException_WhenPhoneDoesNotExist()
        {
            // Arrange
            var employee = GenerateEmployee();
            var nonExistentPhoneId = Guid.NewGuid();
            var newPhoneNumber = _faker.Phone.PhoneNumber();

            // Act
            Action act = () => employee.UpdatePhone(nonExistentPhoneId, newPhoneNumber, PhoneType.ContactEmergency, false);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"Phone with ID {nonExistentPhoneId} not found in the employee.");
        }

        [Fact]
        public void UpdatePhone_ShouldUpdatePhone_WhenPhoneExists()
        {
            // Arrange
            var employee = GenerateEmployee();
            var phone = employee.Phones.First();
            var newPhoneNumber = _faker.Phone.PhoneNumber();

            // Act
            employee.UpdatePhone(phone.Id, newPhoneNumber, PhoneType.ContactEmergency, false);

            // Assert
            phone.Number.Should().Be(newPhoneNumber);
        }

        [Fact]
        public void RemovePhone_ShouldThrowException_WhenPhoneDoesNotExist()
        {
            // Arrange
            var employee = GenerateEmployee();
            var nonExistentPhoneId = Guid.NewGuid();

            // Act
            Action act = () => employee.RemovePhone(nonExistentPhoneId);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"Phone with ID {nonExistentPhoneId} not found in the employee.");
        }

        [Fact]
        public void RemovePhone_ShouldRemovePhone_WhenPhoneExists()
        {
            // Arrange
            var employee = GenerateEmployee();
            var phone = employee.Phones.First();

            // Act
            employee.RemovePhone(phone.Id);

            // Assert
            employee.Phones.Should().NotContain(phone);
        }

        private EmployeeEntity GenerateEmployee()
        {

            var employe =  new EmployeeEntity(
                _faker.Name.FirstName(),
                _faker.Name.LastName(),
                _faker.Internet.Email(),
                _faker.Random.Replace("###########"),
                _faker.Internet.Password(),
                _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                EmployeeRole.Employee                
            );

            var phones = new List<Phone>
            {
                new Phone(employe, _faker.Phone.PhoneNumber(), PhoneType.Home, false),
                new Phone(employe, _faker.Phone.PhoneNumber(), PhoneType.Home, false)
            };

            foreach (var phone in phones)
            {
                employe.AddPhones(phone);
            }

            return employe;
        }
    }
}
