using Employee.Application.Employee.CreateEmployee;
using MediatR;

namespace Employee.Application.Employee.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<UpdateEmployeeResult>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public List<UpdateEmployeePhoneCommand> Phones { get; set; } = new List<UpdateEmployeePhoneCommand>();
    }
    public class UpdateEmployeePhoneCommand
    {
        public Guid? Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
}