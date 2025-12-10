using AutoMapper;
using Azure.Core;
using Employee.Application.Employee.CreateEmployee;
using Employee.Application.Employee.DeleteEmployee;
using Employee.Application.Employee.GetEmployee;
using Employee.Application.Employee.ListEmployees;
using Employee.Application.Employee.UpdateEmployee;
using Employee.WebApi.Commom;
using Employee.WebApi.Controllers.Employee.CreateEmployee;
using Employee.WebApi.Controllers.Employee.DeleteEmployee;
using Employee.WebApi.Controllers.Employee.GetEmployee;
using Employee.WebApi.Controllers.Employee.ListEmployees;
using Employee.WebApi.Controllers.Employee.UpdateEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var authenticatedUserId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var command = _mapper.Map<CreateEmployeeCommand>(request);
            command.AuthenticateUser = authenticatedUserId;

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new { data = _mapper.Map<CreateEmployeeResponse>(response) });
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponseWithData<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployee([FromBody] DeleteEmployeeRequest request)
        {
            var command = _mapper.Map<DeleteEmployeeCommand>(request);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetEmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var command = new GetEmployeeCommand() { EmployeeId = id };
            var result = await _mediator.Send(command);

            return Ok(_mapper.Map<GetEmployeeResponse>(result));
        }


        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<List<ListEmployeesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] ListEmployeesRequest request, CancellationToken cancellationToken = default)
        {
            {
                var command = _mapper.Map<ListEmployeesCommand>(request);
                var item = await _mediator.Send(command, cancellationToken);

                var response = _mapper.Map<List<EmployeeDto>>(item.Employees);

                return Ok(new PaginatedResponse<EmployeeDto>
                {
                    Success = true,
                    Data = response,
                    CurrentPage = request.PageNumber,
                    TotalPages = (int)Math.Ceiling((double)item.TotalCount / request.PageSize),
                    TotalCount = item.TotalCount
                });
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateEmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateEmployeeCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(_mapper.Map<UpdateEmployeeResponse>(response));
        }
    }
}
