using AutoMapper;
using Employee.Application.Employee.CreateEmployee;

namespace Employee.WebApi.Controllers.Employee.CreateEmployee
{
    public class CreateEmployeeProfiler : Profile
    {
        public CreateEmployeeProfiler()
        {
            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>()
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));

            CreateMap<CreateEmployeePhoneRequest, CreateEmployeePhoneCommand>();
        }
    }
}
