using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Employee.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository, ILogger<DeleteEmployeeCommandHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                _logger.LogWarning("Employee - {employeeId} not found.", request.EmployeeId);
                throw new ValidationException(new[] { 
                    new FluentValidation.Results.ValidationFailure(nameof(request.EmployeeId), 
                    $"Employee with ID {request.EmployeeId} not found.") });
            }

            await _employeeRepository.DeleteAsync(employee, cancellationToken);

            _logger.LogInformation("Employee {EmployeeId} was successfully deleted.", request.EmployeeId);


            return true;
        }
    }
}