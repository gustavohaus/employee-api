using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
