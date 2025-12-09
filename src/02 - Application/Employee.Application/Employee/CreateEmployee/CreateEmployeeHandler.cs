using AutoMapper;
using Employee.Common.Validation;
using Employee.Domain.Entities;
using Employee.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
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
            _logger.LogInformation("Starting employee creation. Requested by AuthenticateUser={AuthenticateUser}, Email={Email}, Document={Document}, Role={Role}",
                request.AuthenticateUser, request.Email, request.DocumentNumber, request.Role);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateEmployeeCommand: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _employeeRepository.GetEmployeeByEmailOrCpf(request.Email, request.DocumentNumber);

            if (existingUser != null)
                throw new ValidationException("User already exists with this email or document.");


            var authenticated = await _employeeRepository.GetByIdAsync(request.AuthenticateUser);

            if (authenticated == null)
                throw new ValidationException("Authenticated user not found.");


            if (request.ManagerId == null)
            {
                if (authenticated.Role != EmployeeRole.Admin)
                {
                    _logger.LogWarning("User {UserId} tried to create employee without Admin permission.", authenticated.Id);
                    throw new ValidationException("Only Admin can create an employee without a manager.");
                }
            }
            else
            {
                var manager = await _employeeRepository.GetByIdAsync(request.ManagerId.Value);

                if (manager == null)
                    throw new ValidationException("Manager not found.");

                if (authenticated.Id != manager.Id || (int)request.Role >= (int)authenticated.Role)
                {
                    _logger.LogWarning("manager {managerId} does not create a user with higher permissions than the current one.", request.ManagerId.Value); 
                    throw new ValidationException($"manager {request.ManagerId.Value} does not create a user with higher permissions than the current one.");
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

            _logger.LogInformation("Employee {Email} created successfully.", request.Email);


            return _mapper.Map<CreateEmployeeResult>(employee);
        }
    }
}
