using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Employee.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<CreateEmployeeResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public Guid? ManagerId { get; set; }
        public List<CreateEmployeePhoneCommand> Phones { get; set; } = new List<CreateEmployeePhoneCommand>();
    }
    public class CreateEmployeePhoneCommand
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
}
