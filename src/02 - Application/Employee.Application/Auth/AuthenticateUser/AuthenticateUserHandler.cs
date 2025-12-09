using Employee.Common.Validation;
using Employee.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticateUserHandler(IEmployeeRepository employeeRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
        {
            _employeeRepository = employeeRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticateUserResult?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByEmail(request.Email, cancellationToken);

            if (employee == null || !_passwordHasher.VerifyPassword(request.Password, employee.Password))
            {
                return null;
            }
            var token = _jwtTokenGenerator.GenerateToken(employee.Id, employee.Email, employee.Role.ToString());

            return new AuthenticateUserResult
            {
                Token = token,
                Id = employee.Id
            };
        }
    }
}
