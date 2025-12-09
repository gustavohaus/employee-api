using AutoMapper;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Application.Employee.ListEmployees
{
    public class ListEmployeeProfile : Profile
    {
        public ListEmployeeProfile()
        {
            CreateMap<EmployeeEntity, EmployeeResult>()
                .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager));

            CreateMap<EmployeeEntity, ManagerResult>();
        }
    }
}
