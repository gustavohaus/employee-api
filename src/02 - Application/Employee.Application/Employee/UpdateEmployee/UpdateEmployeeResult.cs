namespace Employee.Application.Employee.UpdateEmployee
{


    public class UpdateEmployeeResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UpdateEmployeeManagerResult? Manager { get; set; }
        public List<UpdateEmployeePhoneResult> Phones { get; set; } = new List<UpdateEmployeePhoneResult>();
    }

    public class UpdateEmployeePhoneResult
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
    public class UpdateEmployeeManagerResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
    }
}