using AutoMapper;
using Employee.Application.Auth.AuthenticateUser;

namespace Employee.WebApi.Controllers.Auth.AuthenticateUser
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
            CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
        }
    }
}
