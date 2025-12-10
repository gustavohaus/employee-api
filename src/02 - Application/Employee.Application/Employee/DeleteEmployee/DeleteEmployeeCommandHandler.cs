using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Employee.DeleteEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DeleteEmployeeHandler> _logger;

        public DeleteEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<DeleteEmployeeHandler> logger)
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
                    $"Employee - {request.EmployeeId} not found.") });
            }

            if (employee.Employees.Any())
            {
                _logger.LogWarning("Employee - {employeeId} cannot be deleted due to active relationships.", request.EmployeeId);
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure(nameof(request.EmployeeId),
                    $"Employee - {request.EmployeeId} cannot be deleted due to active relationships.") });
            }

            await _employeeRepository.DeleteAsync(employee, cancellationToken);

            _logger.LogInformation("Employee {EmployeeId} was successfully deleted.", request.EmployeeId);

            return true;
        }
    }
}