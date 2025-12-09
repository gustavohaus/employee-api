namespace Employee.WebApi.Controllers.Employee.GetEmployee
{
    public class GetEmployeeResponse
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
        public GetEmployeeManagerResponse? Manager { get; set; }
        public List<GetEmployeeManagerResponse> Employees { get; set; } = new List<GetEmployeeManagerResponse>();
        public List<GetEmployeePhoneResponse> Phones { get; set; } = new List<GetEmployeePhoneResponse>();
    }

    public class GetEmployeePhoneResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
    public class GetEmployeeManagerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
    }
}