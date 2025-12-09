namespace Employee.WebApi.Controllers.Employee.ListEmployees
{
    public class ListEmployeesRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ManagerId { get; set; }
        public EmployeeRole? Role { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}