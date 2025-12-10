using AutoMapper;
using Employee.Application.Employee.GetEmployee;
using Employee.Domain.Entities;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Application.Employee.CreateEmployee
{
    public class CreateEmployeeProfiler : Profile
    {
        public CreateEmployeeProfiler()
        {
            CreateMap<EmployeeEntity, CreateEmployeeResult>()
                 .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

            CreateMap<Phone, CreateEmployeePhoneResult>();

            CreateMap<EmployeeEntity, CreateEmployeeManagerResult>();
        }
    }
}
