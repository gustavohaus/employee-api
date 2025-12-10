using MediatR;

namespace Employee.Application.Employee.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid EmployeeId { get; set; }
    }
}