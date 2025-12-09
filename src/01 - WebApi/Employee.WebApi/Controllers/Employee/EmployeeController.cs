using AutoMapper;
using Employee.Application.Employee.CreateEmployee;
using Employee.WebApi.Commom;
using Employee.WebApi.Controllers.Employee.CreateEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Employee.WebApi.Controllers.Employee
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EmployeeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">The employee creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateEmployeeCommand>(request);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
