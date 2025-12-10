using MediatR;

namespace Employee.Application.Employee.GetEmployee
{
    public class GetEmployeeCommand : IRequest<GetEmployeeResult>
    {
        public Guid EmployeeId { get; set; }
    }
}