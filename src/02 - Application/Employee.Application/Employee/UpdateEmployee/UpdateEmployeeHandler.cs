using AutoMapper;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Employee.Application.Employee.UpdateEmployee
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<UpdateEmployeeHandler> _logger;
        private readonly IValidator<UpdateEmployeeCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<UpdateEmployeeHandler> logger,
            IValidator<UpdateEmployeeCommand> validator, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting employee updating Employee - {EmployeeId}", request.Id);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Update: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee == null)
            {
                _logger.LogWarning("Employee - {employeeId} not found.", request.Id);
                throw new ValidationException($"Employee - {request.Id} not found.");
            }

            employee.Update(request.FirstName,
                request.LastName, 
                request.Email, 
                request.DocumentNumber, 
                request.BirthDate,
                request.Role);

            var addPhones = request.Phones.Where(x => x.Id == null);
            var updatePhones = request.Phones.Where(x => x.Id != null);
            var removePhones = employee.Phones.Where(x =>
            !request.Phones.Any(p => p.Id == x.Id)).Select(x => x.Id).ToList();

            foreach (var phone in addPhones)
            {
                employee.AddPhones(new Phone(employee, phone.Number, phone.Type, phone.IsPrimary));
            }

            foreach (var phone in updatePhones)
            {
                employee.UpdatePhone(phone.Id.Value , phone.Number, phone.Type, phone.IsPrimary);
            }

            foreach (var phone in removePhones)
            {
                employee.RemovePhone(phone);
            }

            employee = await _employeeRepository.UpdateAsync(employee, cancellationToken);

            _logger.LogInformation("Employee - {EmployeeId} updated successfully.", request.Id);

            return _mapper.Map<UpdateEmployeeResult>(employee);
        }
    }
}