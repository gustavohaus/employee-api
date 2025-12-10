using Employee.Domain.Entities;

namespace Employee.Application.Employee.GetEmployee
{
    public class GetEmployeeResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
       public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public GetEmployeeManagerResult? Manager { get; set; }
        public List<GetEmployeeManagerResult> Employees { get; set; } = new List<GetEmployeeManagerResult>();
        public List<GetEmployeePhoneResult> Phones { get; set; } = new List<GetEmployeePhoneResult>();
    }

    public class GetEmployeePhoneResult
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
    public class GetEmployeeManagerResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
    }

}