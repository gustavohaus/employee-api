using AutoMapper;
using Employee.Application.Employee.UpdateEmployee;

namespace Employee.WebApi.Controllers.Employee.UpdateEmployee
{
    public class UpdateEmployeeProfile : Profile
    {
        public UpdateEmployeeProfile()
        {
            CreateMap<UpdateEmployeeRequest, UpdateEmployeeCommand>();
            CreateMap<UpdateEmployeePhoneRequest, UpdateEmployeePhoneCommand>();


            CreateMap<UpdateEmployeeResult, UpdateEmployeeResponse>();
            CreateMap<UpdateEmployeePhoneResult, UpdateEmployeePhoneResponse>();
            CreateMap<UpdateEmployeeManagerResult, UpdateEmployeeManagerResponse>();
        }
    }
}