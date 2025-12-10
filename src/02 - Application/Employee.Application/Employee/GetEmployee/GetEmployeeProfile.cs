using AutoMapper;
using Employee.Domain.Entities;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Application.Employee.GetEmployee
{
    public class GetEmployeeProfile : Profile
    {
        public GetEmployeeProfile()
        {
            CreateMap<EmployeeEntity, GetEmployeeResult>();
            CreateMap<Phone, GetEmployeePhoneResult>();
            CreateMap<EmployeeEntity, GetEmployeeManagerResult>();
        }
    }
}
