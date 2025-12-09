using AutoMapper;
using Employee.Common.Validation;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Application.Employee.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResult>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEmployeeHandler> _logger;
        private readonly IValidator<CreateEmployeeCommand> _validator;
        private readonly IPasswordHasher _passwordHasher;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<CreateEmployeeHandler> logger,
            IValidator<CreateEmployeeCommand> validator, IPasswordHasher passwordHasher)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
            _passwordHasher = passwordHasher;
        }
        public async Task<CreateEmployeeResult> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateEmployeeCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = _employeeRepository.GetEmployeeByEmailOrCpf(request.Email, request.DocumentNumber); // email or document has exists.

            if(request.ManagerId != null)
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value);

                if(manager == null || (int)request.Role > (int)manager.Role)
                {
                    _logger.LogWarning("manager {managerId} does not create a user with higher permissions than the current one.", request.ManagerId.Value);
                    throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure(nameof(request.ManagerId.Value), $"manager does not create a user with higher permissions than the current one.") });

                }
            }

            var hash = _passwordHasher.HashPassword(request.Password);

            var employeeEntity = new EmployeeEntity(request.FirstName,
                request.LastName, request.Email, request.DocumentNumber, hash, request.BirthDate, request.Role);

            foreach (var phone in request.Phones)
            {
                employeeEntity.AddPhones(new Phone(employeeEntity, phone.Number, phone.Type, phone.IsPrimary));
            }

             var employee = await _employeeRepository.CreateAsync(employeeEntity, cancellationToken);


            return new CreateEmployeeResult();
        }
    }
}
