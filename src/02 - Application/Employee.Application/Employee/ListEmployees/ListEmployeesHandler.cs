using AutoMapper;
using Employee.Domain.Repositories;
using MediatR;


namespace Employee.Application.Employee.ListEmployees
{
    public class ListEmployeesHandler : IRequestHandler<ListEmployeesCommand, ListEmployeesResult>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public ListEmployeesHandler(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ListEmployeesResult> Handle(ListEmployeesCommand request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetPagedSalesAsync(request.PageNumber, request.PageSize, request.StartDate, request.EndDate, request.ManagerId, request.Role, cancellationToken);

            var employees = _mapper.Map<List<EmployeeResult>>(items.Item1);

            return new ListEmployeesResult()
            {
                Employees = employees,
                TotalCount = items.Item2,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
