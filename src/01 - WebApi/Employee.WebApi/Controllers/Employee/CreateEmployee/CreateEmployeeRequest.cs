using Employee.Domain.Entities;

namespace Employee.WebApi.Controllers.Employee.CreateEmployee
{
    public class CreateEmployeeRequest
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
        public List<CreateEmployeePhoneRequest> Phones { get; set; } = new List<CreateEmployeePhoneRequest>();
    }
    public class CreateEmployeePhoneRequest
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
}
