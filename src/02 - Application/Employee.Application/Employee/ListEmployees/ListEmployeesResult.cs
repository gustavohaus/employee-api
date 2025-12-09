using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Employee.ListEmployees
{
    public class ListEmployeesResult
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<EmployeeResult> Employees { get; set; }
    }
    public class EmployeeResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public ManagerResult Manager { get; set; }
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
    public class ManagerResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmployeeRole Role { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}
