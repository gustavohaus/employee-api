using AutoMapper;
using Employee.Application.Employee.DeleteEmployee;

namespace Employee.WebApi.Controllers.Employee.DeleteEmployee
{
    public class DeleteEmployeeProfile : Profile
    {
        public DeleteEmployeeProfile()
        {
            CreateMap<DeleteEmployeeRequest, DeleteEmployeeCommand>();
        }
    }
}