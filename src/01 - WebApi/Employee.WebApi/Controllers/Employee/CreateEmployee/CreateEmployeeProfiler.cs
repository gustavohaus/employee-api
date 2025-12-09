using AutoMapper;
using Employee.Application.Employee.CreateEmployee;
using Employee.Application.Employee.UpdateEmployee;
using Employee.WebApi.Controllers.Employee.UpdateEmployee;

namespace Employee.WebApi.Controllers.Employee.CreateEmployee
{
    public class CreateEmployeeProfiler : Profile
    {
        public CreateEmployeeProfiler()
        {
            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

            CreateMap<CreateEmployeePhoneRequest, CreateEmployeePhoneCommand>();


            CreateMap<CreateEmployeeResult,        CreateEmployeeResponse>();
            CreateMap<CreateEmployeePhoneResult,   CreateEmployeePhoneResponse>();
            CreateMap<CreateEmployeeManagerResult, CreateEmployeeManagerResponse>();
        }
    }
}
