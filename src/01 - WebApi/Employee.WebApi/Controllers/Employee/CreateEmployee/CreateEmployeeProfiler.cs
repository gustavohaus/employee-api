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


            CreateMap<CreateEmployeeResult,CreateEmployeeResponse>()
                 .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones))
                 .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager));

            CreateMap<CreateEmployeePhoneResult,   CreateEmployeePhoneResponse>();
            CreateMap<CreateEmployeeManagerResult, CreateEmployeeManagerResponse>();

            CreateMap<CreateEmployeeResponse, CreateEmployeeResult>()
                 .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones))
                 .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager));

            CreateMap<CreateEmployeePhoneResponse, CreateEmployeePhoneResult>();
            CreateMap<CreateEmployeeManagerResponse, CreateEmployeeManagerResult>();
        }
    }
}
