using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserResult
    {
        public string Token { get; set; } = string.Empty;
        public Guid Id { get; set; }
    }
}
