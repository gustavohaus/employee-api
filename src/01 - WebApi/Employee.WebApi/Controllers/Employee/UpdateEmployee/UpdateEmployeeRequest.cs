namespace Employee.WebApi.Controllers.Employee.UpdateEmployee
{
    public class UpdateEmployeeRequest
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public Guid? ManagerId { get; set; }
        public List<UpdateEmployeePhoneRequest> Phones { get; set; } = new List<UpdateEmployeePhoneRequest>();
    }
    public class UpdateEmployeePhoneRequest
    {
        public Guid? Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public bool IsPrimary { get; set; }
    }
}