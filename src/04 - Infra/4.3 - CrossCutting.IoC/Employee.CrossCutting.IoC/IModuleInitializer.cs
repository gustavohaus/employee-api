using Microsoft.AspNetCore.Builder;

namespace Employee.CrossCutting.IoC

{
    public interface IModuleInitializer
    {
        void Initialize(WebApplicationBuilder builder);
    }
}
