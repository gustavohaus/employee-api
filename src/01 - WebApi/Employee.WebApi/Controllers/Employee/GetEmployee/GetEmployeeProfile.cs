using AutoMapper;
using Employee.Application.Employee.GetEmployee;

namespace Employee.WebApi.Controllers.Employee.GetEmployee
{
    public class GetEmployeeProfile : Profile
    {   
        public GetEmployeeProfile()
        {
            CreateMap<GetEmployeeResult, GetEmployeeResponse>();
            CreateMap<GetEmployeePhoneResult, GetEmployeePhoneResponse>();
            CreateMap<GetEmployeeManagerResult, GetEmployeeManagerResponse>();
        }
    }
}