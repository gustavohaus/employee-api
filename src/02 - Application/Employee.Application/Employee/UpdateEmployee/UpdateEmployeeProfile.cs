using AutoMapper;
using Employee.Domain.Entities;
using EmployeeEntity = Employee.Domain.Entities.Employee;

namespace Employee.Application.Employee.UpdateEmployee
{
    public class UpdateEmployeeProfile : Profile
    {
        public UpdateEmployeeProfile()
        {
            CreateMap<EmployeeEntity, UpdateEmployeeResult>();
            CreateMap<EmployeeEntity, UpdateEmployeeManagerResult>();
            CreateMap<Phone, UpdateEmployeePhoneResult>();
        }
    }
}
