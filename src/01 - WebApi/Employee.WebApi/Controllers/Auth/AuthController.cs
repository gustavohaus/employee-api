using AutoMapper;
using Employee.Application.Auth.AuthenticateUser;
using Employee.WebApi.Controllers.Auth.AuthenticateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
        {
            var command = _mapper.Map<AuthenticateUserCommand>(request);
            var result = await _mediator.Send(command);

            if (result == null)
                return Unauthorized();

            var response = _mapper.Map<AuthenticateUserResponse>(result);
            return Ok(response);
        }
    }
}
