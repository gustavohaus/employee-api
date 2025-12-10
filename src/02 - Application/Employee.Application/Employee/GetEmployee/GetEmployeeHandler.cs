using AutoMapper;
using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Employee.GetEmployee
{
    public class GetEmployeeHandler : IRequestHandler<GetEmployeeCommand, GetEmployeeResult>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEmployeeHandler> _logger;

        public GetEmployeeHandler(IEmployeeRepository repository, IMapper mapper, ILogger<GetEmployeeHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetEmployeeResult> Handle(GetEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                _logger.LogWarning("Employee - {employeeId} not found.", request.EmployeeId);
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure(nameof(request.EmployeeId),
                    $"Employee with ID {request.EmployeeId} not found.") });
            }

            return _mapper.Map<GetEmployeeResult>(employee);
        }
    }
}