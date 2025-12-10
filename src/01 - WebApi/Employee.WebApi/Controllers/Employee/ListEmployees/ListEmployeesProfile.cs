using AutoMapper;
using Employee.Application.Employee.ListEmployees;

namespace Employee.WebApi.Controllers.Employee.ListEmployees
{
    public class ListEmployeesProfile : Profile
    {
        public ListEmployeesProfile()
        {
            CreateMap<ListEmployeesRequest, ListEmployeesCommand>();
            CreateMap<EmployeeResult, EmployeeDto>();
            CreateMap<ManagerResult, ManagerDto>();

        }
    }
}